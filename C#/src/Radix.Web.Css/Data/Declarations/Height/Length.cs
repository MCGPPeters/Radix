using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Web.Css.Data.Dimensions;

namespace Radix.Web.Css.Data.Declarations.Height;

public record Length : Declaration<Properties.Height.Values.Length>
{
    public Radix.Web.Css.Data.Properties.Height.Values.Length Value { get; init; }
}

