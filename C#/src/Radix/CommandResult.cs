namespace Radix;

public struct CommandResult<TEvent>
{
    public Id Id { get; init; }

    public Version ExpectedVersion { get; init; }

    public TEvent[] Events { get; init; }
}
