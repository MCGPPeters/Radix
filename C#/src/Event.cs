
namespace Radix
{
    public abstract class Event
    {
        protected Event(Address address)
        {
            Address = address;
        }

        public override string ToString()
        {
            var typeName = GetType().ToString();
            return char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
        }

        public Address Address { get; }
    }
}
