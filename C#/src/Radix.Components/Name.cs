namespace Radix.Components;

public record struct Name(string value) : Alias<Name, string>
{
    public static implicit operator Name(string value) => new(value);

    public static implicit operator string(Name name) => name;

}
