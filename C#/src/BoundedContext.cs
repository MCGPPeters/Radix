using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
        private readonly Dictionary<Address, Agent<TCommand>> _registry = new Dictionary<Address, Agent<TCommand>>();
        private readonly Timer _timer;

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings)
        {
            _boundedContextSettings = boundedContextSettings;
            _timer = new Timer(RunGarbageCollection, null, new TimeSpan(0), new TimeSpan(0,  boundedContextSettings.GarbageCollectionSettings.ScanInterval.Value, 0, 0));
        }

        private void RunGarbageCollection(object state)
        {
            foreach (var (address, agent) in _registry)
            {
                var idleTime = DateTimeOffset.Now - agent.LastActivity;
                if (idleTime >= _boundedContextSettings.GarbageCollectionSettings.IdleTimeout)
                {
                    agent.Deactivate();
                    _registry.Remove(address);
                }
            }
        }


        public Address CreateAggregate<TState>(TaskScheduler scheduler)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var address = new Address(Guid.NewGuid());

            var agent = new StatefulAgent<TState, TCommand, TEvent>(_boundedContextSettings, Array.Empty<EventDescriptor<TEvent>>(), scheduler);

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
            where TState : Aggregate<TState, TEvent, TCommand>, new() =>
            CreateAggregate<TState>(TaskScheduler.Default);

        public async Task Send<TState>(CommandDescriptor<TCommand> commandDescriptor)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {

            if (!_registry.TryGetValue(commandDescriptor.Address, out var agent))
            {
                agent = await GetAggregate<TState>(commandDescriptor.Address);
            }

            agent.Post(commandDescriptor);

        }

        private async Task<StatefulAgent<TState, TCommand, TEvent>> GetAggregate<TState>(Address address)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
            => await GetAggregate<TState>(address, TaskScheduler.Default);

        /// <summary>
        ///     Spins up an agent for an existing aggregate and restores its last known state
        /// </summary>
        /// <typeparam name="TState">The type of the aggregate</typeparam>
        /// <param name="address">The address of the aggregate</param>
        /// <param name="scheduler">The task scheduler to use</param>
        /// <returns></returns>
        private async Task<StatefulAgent<TState, TCommand, TEvent>> GetAggregate<TState>(Address address, TaskScheduler scheduler)
            where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var history = await _boundedContextSettings.GetEventsSince(address, new Version(0L));
            var agent = new StatefulAgent<TState, TCommand, TEvent>(_boundedContextSettings, history, scheduler);
            _registry.Add(address, agent);
            return agent;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }

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