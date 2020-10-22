using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Math.Applied.Learning.Reinforced
{
    public record Reward(double Value) : Alias<double>(Value);
}
