using Radix.Monoid;

namespace Radix
{

    /// <summary>
    ///     An error while save to the event store
    /// </summary>
    public class AppendEventsError : CommandProcessingError
    {
    }

    public interface CommandProcessingError
    {
    }


}
