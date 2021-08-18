namespace Radix.Option;

public sealed class None<T> : Option<T>
{
    internal static readonly None<T> Default = new();
}
