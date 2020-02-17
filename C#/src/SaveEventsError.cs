namespace Radix
{
    /// <summary>
    ///     An error while save to the event store
    /// </summary>
    public interface SaveEventsError : Monoid<SaveEventsError>
    {
    }
}
