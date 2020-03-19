namespace Radix.Blazor.Html
{
    public class ComponentAttribute : IAttribute
    {
        public ComponentAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public object Value { get; }

        public Name Name { get; set; }

        public void Deconstruct(out Name name, out object value)

        {
            name = Name;
            value = Value;
        }
    }
}
