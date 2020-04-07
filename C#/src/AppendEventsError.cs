namespace Radix
{

    /// <summary>
    ///     An error while save to the event store
    /// </summary>
    public class AppendEventsError : CommandProcessingError
    {
        internal AppendEventsError(string Message) : base(Message)
        {
        }
    }


}
