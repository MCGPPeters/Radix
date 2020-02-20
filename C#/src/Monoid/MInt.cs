namespace Radix.Monoid
{
    public readonly struct MInt : Monoid<MInt>, Value<int>
    {
        private MInt(in int i)
        {
            Value = i;
        }

        public MInt Append(MInt x, MInt y)
        {
            return x + y;
        }

        public MInt Empty()
        {
            return 0;
        }

        public static implicit operator MInt(int s)
        {
            return new MInt(s);
        }

        public static implicit operator int(MInt mInt)
        {
            return mInt.Value;
        }

        public int Value { get; }
    }
}
