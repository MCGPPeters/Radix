using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Interaction;

public static class Attribute
{
    public static Data.Attribute Create<T>(string[] values, [CallerLineNumber] int nodeId = 0)
        where T : Literal<T>, AttributeName
        => () =>
        (_, builder)
            =>
        {
            if (values.Length > 0)
                builder.AddAttribute(nodeId, T.Format(), values.Aggregate((current, next) => $"{current} {next}"));

        };

    public static Data.Attribute Create(string name, object[] values, [CallerLineNumber] int nodeId = 0)
        => () =>
            (_, builder)
                =>
            {
                if (values.Length > 0)
                    builder.AddAttribute(nodeId, name, values.Aggregate((current, next) => $"{current} {next}"));

            };
}
