namespace Radix.Components.Html;

public class Attribute : IAttribute
{
    public Attribute(string name, params string[] values)
    {

        Name = name;
        Values = values ?? Array.Empty<string>();
    }

    public IEnumerable<string> Values { get; set; }

    public string Name { get; set; }

    public void Deconstruct(out string name, out IEnumerable<string> values)

    {
        name = Name;
        values = Values;
    }
}
