namespace Radix.Web.Html.Data;

public class Attribute : Components.Attribute
{
    public Attribute(string name, IEnumerable<string> values)
    {
        Name = name;
        Values = values;
    }

    public string Name { get; init; }
    public IEnumerable<string> Values { get; set; }
}

public class Attribute<T> : Attribute
    where T : Literal<T>, AttributeName
{
    public Attribute(params string[] values) : base(T.Format(), values)
    {
    }

    public IEnumerable<string> Values { get; set; }

    public string Name { get; init; }

    public void Deconstruct(out string name, out IEnumerable<string> values)

    {
        name = Name;
        values = Values;
    }
}
