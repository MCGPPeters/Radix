﻿using Radix.Interaction.Data;
using Radix.Web.Html.Data;

namespace Radix.Interaction.Web.Components;

public record Attribute<T> : Data.Attribute<string>
    where T : AttributeName, Literal<T>
{
    public Attribute(NodeId NodeId, params string[] values) : base(NodeId, T.Format(), values)
    {
    }

    public void Deconstruct(out string name, out IEnumerable<string> values)
    {
        name = Name;
        values = Values;
    }
}
