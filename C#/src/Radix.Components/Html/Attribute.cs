namespace Radix.Components.Html;

public class Attribute : IAttribute
{
    public Attribute(Name name, params string[] values)
    {

        Name = name;
        Values = values ?? Array.Empty<string>();
    }

    public IEnumerable<string> Values { get; set; }

    public Name Name { get; set; }

    public void Deconstruct(out Name name, out IEnumerable<string> values)

    {
        name = Name;
        values = Values;
    }
}
