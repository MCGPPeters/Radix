namespace Radix.Monoid
{
    public record MInt(in int i): Alias<int>(i), Monoid<MInt>
    {
        

        public MInt Append(MInt x, MInt y) => x + y;

        public MInt Empty() => 0;

        public static implicit operator MInt(int s) => new MInt(s);
        public static implicit operator int(MInt s) => s.Value;

    }
}
