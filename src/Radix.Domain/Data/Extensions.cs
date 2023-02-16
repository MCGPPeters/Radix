using Radix.Control.Task.Result;
using Radix.Control.Validated;
using Radix.Data;
using System.Diagnostics.Contracts;
using static Radix.Prelude;

namespace Radix.Domain.Data;

public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="command"></param>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TAggregateCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TAggregateEvent"></typeparam>
    /// <typeparam name="TEventStore"></typeparam>
    /// <returns></returns>
    [Pure]
    public static Task<Validated<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>>>
        Handle<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>(
            this Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings> instance,
            Validated<TAggregateCommand> command)
        where TEventStore : EventStore<TEventStore, TEventStoreSettings>
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent
    {
        return command
            .Select(async cmd => (await instance.Context.Handle(instance, cmd)))
            .Traverse(id => id);
    }
            

}
