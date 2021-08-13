namespace Radix.Validated;

public record Invalid<T>(params string[] Reasons) : Validated<T>
{
    public static implicit operator Invalid<T>(string[] reasons) => new(reasons);

    public static implicit operator string[](Invalid<T> invalid) => invalid.Reasons;
}
