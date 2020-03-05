
namespace Radix
{
    public abstract class Event
    {
        protected Event(Address address)
        {
            Address = address;

        }
        public Address Address { get; }
    }
}
