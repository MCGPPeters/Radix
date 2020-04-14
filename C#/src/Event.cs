namespace Radix
{
    public abstract class Event
    {
        protected Event(Address aggregate)
        {
            Aggregate = aggregate;
        }

        public Address Aggregate { get; }

        public override string ToString()
        {
            string typeName = GetType().ToString();
            return char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
        }
    }
}
