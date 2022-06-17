namespace Radix.Domain.Data;

public struct CommandResult<TEvent>
{
    public Aggregate.Id Id { get; init; }

    public Version ExpectedVersion { get; init; }

    public TEvent[] Events { get; init; }
}
