namespace Radix
{
    public readonly struct ExistingVersion : Version, Value<long>
    {
        public ExistingVersion(long value) => Value = value;


        public static implicit operator ExistingVersion(long value) => new ExistingVersion(value);

        public static implicit operator long(ExistingVersion existingVersion) => existingVersion.Value;

        public long Value { get; }

    }
}
