namespace Radix.Data;

public interface Validity<T>
{
    public static abstract Func<string, Func<T, Validated<T>>> Validate { get; }
}

public interface Validity<T, P>
{
    public static abstract P Parameter { get; set; }

    public static abstract Func<string, Func<T, Validated<T>>> Validate { get; }
}
