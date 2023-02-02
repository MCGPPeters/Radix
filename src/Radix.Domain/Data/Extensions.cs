using Radix.Data;
using System.Diagnostics.Contracts;

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
    public static Task<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore>>
        Handle<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore>(this Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore> instance,
            Validated<TAggregateCommand> command)
        where TEventStore : EventStore<TEventStore>
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>, new()
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent
            => instance.Context.Handle(instance, command);

}
