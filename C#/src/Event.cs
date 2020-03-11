namespace Radix
{
    public abstract class Event
    {
        protected Event(Address address)
        {
            Address = address;
        }

        public Address Address { get; }

        public override string ToString()
        {
            var typeName = GetType().ToString();
            return char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
        }
    }
}
