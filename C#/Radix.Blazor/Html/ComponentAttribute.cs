namespace Radix.Blazor.Html
{
    public class ComponentAttribute : IAttribute
    {
        public ComponentAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public Name Name { get; set; }
        public object Value { get; }

        public void Deconstruct(out Name name, out object value)

        {
            name = Name;
            value = Value;
        }
    }
}
