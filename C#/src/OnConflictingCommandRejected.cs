using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    ///     Called when a true concurrency conflict according to business rules had occured.
    ///     No events have been recorded that would have been generated as a consequence of
    ///     the command if it would have succeeded. Handle the conflict in a way that makes sence
    ///     to your scenario. For instance log it.
    /// </summary>
    /// <param name="conflicts"></param>
    /// <returns>Unit</returns>
    public delegate Task<Unit> OnConflictingCommandRejected<TCommand, TEvent>(Conflict<TCommand, TEvent> conflict) where TEvent : Event;
}
