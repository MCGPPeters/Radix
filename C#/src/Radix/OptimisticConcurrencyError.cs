namespace Radix
{
    /// <summary>
    ///     The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    public class OptimisticConcurrencyError : AppendEventsError
    {

        public OptimisticConcurrencyError(string message) : base(message)
        {
        }
    }
}
