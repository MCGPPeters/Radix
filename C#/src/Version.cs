namespace Radix
{
    public readonly struct Version : IVersion, Value<long>
    {
        public Version(long value)
        {
            Value = value;
        }


        public static implicit operator Version(long value)
        {
            return new Version(value);
        }

        public static implicit operator long(Version version)
        {
            return version.Value;
        }

        public long Value { get; }

    }
}
