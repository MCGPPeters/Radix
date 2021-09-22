namespace Radix.Components.Html;

public class ComponentAttribute : IAttribute
{
    public ComponentAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public object Value { get; }

    public string Name { get; set; }

    public void Deconstruct(out string name, out object value)

    {
        name = Name;
        value = Value;
    }
}
