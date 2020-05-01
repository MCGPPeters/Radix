using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Radix.Monoid;
using Radix.Validated;

namespace Radix
{

    /// <summary>
    ///     The bounded context is responsible for managing the runtime.
    ///     - Once an aggregate is created, it is never destroyed
    ///     - An aggregate can only acquire an address of an other aggregate when it is explicitly send to it or it has created
    ///     it
    ///     - The runtime is responsible to restoring the state of the aggregate when it is not alive within
    ///     the context or any other remote instance of the context within a cluster
    ///     - Only one instance of an aggregate will be alive
    ///     - One can only send commands that are scoped to the bounded context.
    ///     - All commands that are scoped to an aggregate MUST be subtypes of the command type scoped at the bounded context
    ///     level
    ///     - There is only one command type scoped and the level of the bounded context.
    ///     All commands scoped to an aggregate MUST be subtypes of that command type
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public class BoundedContext<TCommand, TEvent> : IDisposable where TEvent : Event where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly Dictionary<Address, Agent<TCommand, TEvent>> _registry = new Dictionary<Address, Agent<TCommand, TEvent>>();
        private readonly Timer _timer;

        private bool _disposedValue; // To detect redundant calls

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings)
        {
            _boundedContextSettings = boundedContextSettings;
            _timer = new Timer(boundedContextSettings.GarbageCollectionSettings.ScanInterval.TotalMilliseconds) {AutoReset = true};
            _timer.Elapsed += RunGarbageCollection;
            _timer.Enabled = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() => Dispose(true);

        private void RunGarbageCollection(object sender, ElapsedEventArgs e)
        {
            List<Address> deactivatedAgents = new List<Address>();
            foreach ((Address address, Agent<TCommand, TEvent> agent) in _registry)
            {
                TimeSpan idleTime = DateTimeOffset.Now.Subtract(agent.LastActivity);
                if (idleTime >= _boundedContextSettings.GarbageCollectionSettings.IdleTimeout)
                {
                    agent.Deactivate();
                    deactivatedAgents.Add(address);
                }
            }

            foreach (Address deactivatedAgent in deactivatedAgents)
            {
                _registry.Remove(deactivatedAgent);
            }
        }


        public async Task<Aggregate<TCommand, TEvent>> Create<TState>(Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
            where TState : new()
        {
            Address address = new Address(Guid.NewGuid());

            AggregateAgent<TState, TCommand, TEvent> agent = await AggregateAgent<TState, TCommand, TEvent>
                .Create(address, _boundedContextSettings, decide, update)
                .ConfigureAwait(false);

            _registry.Add(address, agent);
            return new Aggregate<TCommand, TEvent>(address, CreateSend<TState>()(address, decide, update));
        }


        private Func<Address, Decide<TState, TCommand, TEvent>, Update<TState, TEvent>, Send<TCommand, TEvent>> CreateSend<TState>()
            where TState : new() => (address, decide, update)
            => async validatedCommand =>
            {
                switch (validatedCommand)
                {
                    case Valid<TCommand>(var validCommand):
                        TransientCommandDescriptor<TCommand> transientCommandDescriptor = new TransientCommandDescriptor<TCommand>(address, validCommand);
                        if (!_registry.TryGetValue(transientCommandDescriptor.Recipient, out Agent<TCommand, TEvent> agent))
                        {
                            agent = await AggregateAgent<TState, TCommand, TEvent>.Create(address, _boundedContextSettings, decide, update);
                            _registry.Add(transientCommandDescriptor.Recipient, agent);
                        }

                        return await agent.Post(transientCommandDescriptor).ConfigureAwait(false);
                    case Invalid<TCommand>(var messages):
                        return new Error<TEvent[], Error[]>(messages.Select(s => new Error(s)).ToArray());
                    default: throw new InvalidOperationException();
                }
            };

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
