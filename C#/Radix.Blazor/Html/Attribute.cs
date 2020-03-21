using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public struct Attribute : IAttribute
    {
        public Attribute(Name name, params string[] values)
        {
            Name = name;
            Values = values;
        }

        public Name Name { get; set; }
        public string[] Values { get; set; }

        public void Deconstruct(out Name name, out string[] values)

        {
            name = Name;
            values = Values;
        }
    }
}
