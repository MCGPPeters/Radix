namespace Radix
{
    public abstract record Alias<T>(T Value)
    {
        public static implicit operator T(Alias<T> alias) => alias.Value;

        public void Deconstruct(out T value) => value = Value;
    }
}
