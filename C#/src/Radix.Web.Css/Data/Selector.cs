using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Web.Css.Data;

public interface Rule
{
    public Selector Selector { get; set; }
    public IEnumerable<Declaration> Declarations { get; set; }
}
