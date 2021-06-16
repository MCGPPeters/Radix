namespace Radix
{
    public abstract record Alias<T>(T Value) where T: notnull
    {
        public static implicit operator T(Alias<T> alias) => alias.Value;

        public override string ToString() => Value.ToString() ?? string.Empty;
    }
}
