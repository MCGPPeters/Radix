using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Web.Css.Data
{
    public interface Selector { }

    [Literal]
    public partial struct a : Selector { }

    [Literal]
    public partial struct b : Selector { }

    [Literal]
    public partial struct c : Selector { }

}
