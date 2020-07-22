namespace Radix
{
    public abstract record Alias<T>(T Value)
    {
        public static implicit operator T(Alias<T> alias) => alias.Value;
    }
}
