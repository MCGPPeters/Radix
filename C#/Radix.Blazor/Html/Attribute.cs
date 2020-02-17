using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public struct Attribute : IAttribute
    {
        public Attribute(Name name, IEnumerable<string> values)
        {
            Name = name;
            Values = values;
        }

        public Name Name { get; set; }
        IEnumerable<string> Values { get; set; }
    }
}