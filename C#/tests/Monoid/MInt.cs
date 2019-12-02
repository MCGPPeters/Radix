namespace Radix.Tests.Monoid
{
    public class MInt : Monoid<MInt>
    {
        private MInt(in int i)
        {
            Value = i;
        }

        public MInt Append(MInt x, MInt y) => x + y;

        public MInt Empty() => 0;

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
