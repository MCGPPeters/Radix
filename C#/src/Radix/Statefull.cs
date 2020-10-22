using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix
{
    public delegate (V, S) Statefull<S, V>(S state);
}
