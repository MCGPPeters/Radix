namespace Radix
{
    public readonly struct AnyVersion : Version, Value<long>
    {
        public long Value => -2;
    }
}
