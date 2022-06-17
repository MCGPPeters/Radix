namespace Radix.Domain.Data;

[Serializable]
public class EventStreamDescriptor
{
    private const char Separator = '#';

    public EventStreamDescriptor()
    {

    }
    public EventStreamDescriptor(string? aggregateType, Aggregate.Id id)
    {
        AggregateType = aggregateType;
        AggregateId = id;
    }

    public static EventStreamDescriptor FromString(string identifier)
    {
        string[]? result = identifier.Split(Separator);
        return new EventStreamDescriptor() { AggregateType = result[0], AggregateId = (Aggregate.Id)Guid.Parse(result[1]) };
    }


    public string StreamIdentifier => $"{AggregateType}{Separator}{AggregateId}";

    private string? AggregateType { get; init; }
    public Aggregate.Id? AggregateId { get; init; }
}

//[Parsed<EventStreamDescriptor, Radix.Data.Int.Read>]
//public partial record PriceFraction { }
