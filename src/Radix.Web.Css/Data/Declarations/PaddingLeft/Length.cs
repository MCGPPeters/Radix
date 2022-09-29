using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Web.Css.Data.Dimensions;

namespace Radix.Web.Css.Data.Declarations.PaddingLeft;

public record Length : Declaration<Dimensions.Length>
{
    public new Dimensions.Length? Value { get; init; }
}

