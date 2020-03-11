using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

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
    public class BoundedContext<TCommand, TEvent> : IDisposable where TEvent : Event
    {
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly Dictionary<Address, Agent<TCommand>> _registry = new Dictionary<Address, Agent<TCommand>>();
        private readonly TaskScheduler _taskScheduler;
        private readonly Timer _timer;

        private bool disposedValue; // To detect redundant calls

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler taskScheduler)
        {
            _boundedContextSettings = boundedContextSettings;
            _taskScheduler = taskScheduler;
            _timer = new Timer(boundedContextSettings.GarbageCollectionSettings.ScanInterval.TotalMilliseconds) {AutoReset = true};
            _timer.Elapsed += RunGarbageCollection;
            _timer.Enabled = true;
        }

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings) : this(boundedContextSettings, TaskScheduler.Default)
        {

        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        private void RunGarbageCollection(object sender, ElapsedEventArgs e)
        {
            var deactivatedAgents = new List<Address>();
            foreach (var (address, agent) in _registry)
            {
                var idleTime = DateTimeOffset.Now.Subtract(agent.LastActivity);
                if (idleTime >= _boundedContextSettings.GarbageCollectionSettings.IdleTimeout)
                {
                    agent.Deactivate();
                    deactivatedAgents.Add(address);
                }
            }

            foreach (var deactivatedAgent in deactivatedAgents)
                _registry.Remove(deactivatedAgent);
        }

        public async Task<Address> CreateAggregate<TState>()
            where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new()
        {
            var address = new Address(Guid.NewGuid());

            var agent = await AggregateAgent<TState, TCommand, TEvent>.Create(_boundedContextSettings, Array.Empty<EventDescriptor<TEvent>>().ToAsyncEnumerable(), _taskScheduler);

            _registry.Add(address, agent);
            return address;
        }

        /// <summary>
        ///     Creates a new aggregate that schedules work using the default task scheduler (TaskScheduler.Default)
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        public async Task<Address> GetAggregate<TState>()
            where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new()
        {
            return await CreateAggregate<TState>();
        }

        public async Task Send<TState>(CommandDescriptor<TCommand> commandDescriptor)
            where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new()
        {

            if (!_registry.TryGetValue(commandDescriptor.Address, out var agent))
                agent = await GetAggregate<TState>(commandDescriptor.Address);

            await agent.Post(commandDescriptor);

        }

        private async Task<AggregateAgent<TState, TCommand, TEvent>> GetAggregate<TState>(Address address)
            where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new()
        {
            var history = _boundedContextSettings.EventStore.GetEventsSince(address, new Version(0L));

            var agent = await AggregateAgent<TState, TCommand, TEvent>.Create(_boundedContextSettings, history, _taskScheduler);
            _registry.Add(address, agent);
            return agent;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    _timer.Dispose();

                disposedValue = true;
            }
        }
    }
}
