using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Web.Css.Data
{
    public interface Declaration
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    public interface Rule
    {
        public Selector Selector { get; set; }
        public IEnumerable<Declaration> Declarations { get; set; }
    }



    public interface Style{}

    public interface Selector { }

    [Literal]
    public partial struct a : Selector { }

    [Literal]
    public partial struct b : Selector { }

    [Literal]
    public partial struct c : Selector { }

}
