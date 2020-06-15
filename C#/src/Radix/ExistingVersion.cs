namespace Radix
{
    public class ExistingVersion : Version, Value<long>
    {
        private ExistingVersion(long value) => Value = value;

        public long Value { get; }


        public static implicit operator ExistingVersion(long value) => new ExistingVersion(value);

        public static implicit operator long(ExistingVersion existingVersion) => existingVersion.Value;
    }
}
