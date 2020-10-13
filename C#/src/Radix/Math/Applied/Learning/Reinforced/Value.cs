using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Learning.Reinforced
{
    /// <summary>
    /// State or utility value
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public record U<S>(Random<S> Value) : Expectation<S>(Value);

    /// <summary>
    /// Action value
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public record Q<A>(Random<A> Value) : Expectation<A>(Value);

}
