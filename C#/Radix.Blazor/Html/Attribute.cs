using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public struct Attribute
    {
        public Attribute(Name name, IEnumerable<NonNullString> values)
        {
            Name = name;
            Values = values;
        }

        Name Name { get; set; }
        IEnumerable<NonNullString> Values { get; set; }
    }
}