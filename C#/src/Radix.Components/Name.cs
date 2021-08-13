namespace Radix.Components;

public record Name(string value) : Alias<string>(value)
{
    public static implicit operator Name(string value) => new(value);
}
