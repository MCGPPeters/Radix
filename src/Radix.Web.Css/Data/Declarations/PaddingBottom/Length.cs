﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Web.Css.Data.Dimensions;

namespace Radix.Web.Css.Data.Declarations.PaddingBottom;

public record Length<T, U> : Declaration<Length<U>>
    where U : Literal<U>, Units.Length.Unit<U>
    where T : Literal<Property>
{
    public new Properties.PaddingBottom.Value<Length<U>>? Value { get; init; }
}

