namespace Radix;

[Serializable]
public class EventStreamDescriptor
{
    private const char Separator = '#';

    public EventStreamDescriptor()
    {

    }

    public static EventStreamDescriptor FromString(string identifier)
    {
        string[]? result = identifier.Split(Separator);
        return new EventStreamDescriptor() { AggregateType = result[0], AggregateId = new Id(Guid.Parse(result[1])) };
    }

    public EventStreamDescriptor(string? aggregateType, Id id)
    {
        AggregateType = aggregateType;
        AggregateId = id;
    }

    public string StreamIdentifier => $"{AggregateType}{Separator}{AggregateId}";

    private string? AggregateType { get; init; }
    public Id? AggregateId { get; init; }
}
