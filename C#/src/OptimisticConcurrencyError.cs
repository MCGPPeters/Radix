namespace Radix
{
    /// <summary>
    ///     The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    public class OptimisticConcurrencyError : SaveEventsError
    {
        public SaveEventsError Append(SaveEventsError x, SaveEventsError y)
        {
            return new OptimisticConcurrencyError();
        }

        public SaveEventsError Empty()
        {
            return new OptimisticConcurrencyError();
        }
    }
}
