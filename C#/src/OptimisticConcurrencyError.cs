using System;

namespace Radix
{
    /// <summary>
    ///     The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    public class OptimisticConcurrencyError : SaveEventsError
    {
        public SaveEventsError Append(SaveEventsError x, SaveEventsError y)
        {
            throw new NotImplementedException();
        }

        public SaveEventsError Empty()
        {
            throw new NotImplementedException();
        }
    }
}
