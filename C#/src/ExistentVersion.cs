namespace Radix
{
    public readonly struct ExistentVersion : Version, Value<long>
    {
        public ExistentVersion(long value) => Value = value;


        public static implicit operator ExistentVersion(long value) => new ExistentVersion(value);

        public static implicit operator long(ExistentVersion existentVersion) => existentVersion.Value;

        public long Value { get; }

    }
}
