namespace Radix.Option
{
    public readonly struct None<T> : Option<T>
    {
        internal static readonly None<T> Default = new None<T>();
    }
}
