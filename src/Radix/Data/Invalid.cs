namespace Radix.Data;

public sealed record Invalid<T>(params Reason[] Reasons) : Validated<T>
{
    public Invalid(params string[] reasons) : this(reasons.Select(reason => new Reason("Invalid value", reason)).ToArray())
    {

    }

    public static implicit operator Invalid<T>(Reason[] reasons) => new(reasons);

    public static implicit operator Reason[] (Invalid<T> invalid) => invalid.Reasons;
}
