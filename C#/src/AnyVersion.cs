namespace Radix
{
    public readonly struct AnyVersion : IVersion, Value<long>
    {
        public long Value => -2;
    }
}