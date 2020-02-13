using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public struct Attribute : IAttribute
    {
        public Attribute(Name name, IEnumerable<NonNullString> values)
        {
            Name = name;
            Values = values;
        }

        public Name Name { get; set; }
        IEnumerable<NonNullString> Values { get; set; }
    }
}