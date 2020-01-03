namespace Radix.Monoid
{
    public readonly struct MInt : Monoid<MInt>, Value<int>
    {
        private MInt(in int i) => Value = i;

        public MInt Append(MInt x, MInt y) => x + y;

        public MInt Empty() => 0;

        public static implicit operator MInt(int s) => new MInt(s);

        public static implicit operator int(MInt mInt) => mInt.Value;

        public int Value { get; }
    }
}
