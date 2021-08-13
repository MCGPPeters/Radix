namespace Radix.Option;

public class None<T> : Option<T>
{
    internal static readonly None<T> Default = new();
}
