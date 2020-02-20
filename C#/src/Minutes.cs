namespace Radix
{
    public struct Minutes : Value<int>
    {
        public Minutes(int value)
        {
            Value = value;

        }
        public int Value { get; }
    }
}
