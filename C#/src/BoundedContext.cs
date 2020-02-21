using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading.Tasks;

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
    public class BoundedContext<TCommand, TEvent> : IDisposable
    {
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly TaskScheduler _taskScheduler;
        private readonly Dictionary<Address, Agent<TCommand>> _registry = new Dictionary<Address, Agent<TCommand>>();
        private readonly Timer _timer;

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler taskScheduler)
        {
            _boundedContextSettings = boundedContextSettings;
            _taskScheduler = taskScheduler;
            _timer = new Timer(boundedContextSettings.GarbageCollectionSettings.ScanInterval.TotalMilliseconds) {AutoReset = true};
            _timer.Elapsed += RunGarbageCollection;
            _timer.Enabled = true;
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
            {
                _registry.Remove(deactivatedAgent);
            }
        }

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings) : this(boundedContextSettings, TaskScheduler.Default)
        {

        }

        public Address CreateAggregate<TState>()
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var address = new Address(Guid.NewGuid());

            var agent = new StatefulAgent<TState, TCommand, TEvent>(_boundedContextSettings, Array.Empty<EventDescriptor<TEvent>>(), _taskScheduler);

            _registry.Add(address, agent);
            return address;
        }

        /// <summary>
        ///     Creates a new aggregate that schedules work using the default task scheduler (TaskScheduler.Default)
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        public Address GetAggregate<TState>()
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            return CreateAggregate<TState>();
        }

        public async Task Send<TState>(CommandDescriptor<TCommand> commandDescriptor)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {

            if (!_registry.TryGetValue(commandDescriptor.Address, out var agent))
                agent = await GetAggregate<TState>(commandDescriptor.Address);

            await agent.Post(commandDescriptor);

        }

        private async Task<StatefulAgent<TState, TCommand, TEvent>> GetAggregate<TState>(Address address)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var history = await _boundedContextSettings.GetEventsSince(address, new Version(0L));
            var agent = new StatefulAgent<TState, TCommand, TEvent>(_boundedContextSettings, history, _taskScheduler);
            _registry.Add(address, agent);
            return agent;
        }


        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    _timer.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BoundedContext()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
