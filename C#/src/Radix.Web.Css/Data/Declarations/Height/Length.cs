using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Web.Css.Data.Dimensions;

namespace Radix.Web.Css.Data.Declarations.Height;

public struct Length<T, U> : Declaration<Length<U>>
    where U : Literal<U>, Units.Length.Unit<U>
    where T : Literal<Property>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Length<U>> Value { get; init; }
}

