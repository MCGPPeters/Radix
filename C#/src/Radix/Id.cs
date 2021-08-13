namespace Radix;

/// <summary>
///     A reference to and identifier of an aggregate
/// </summary>
public record Id(Guid id)
{
    public static implicit operator Id(Guid guid) => new(guid);

    public override string ToString() => id.ToString();
}
