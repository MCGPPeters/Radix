using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Probability;


namespace Radix.Math.Applied.Optimization
{
    /// <summary>
    ///     Action value function
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public delegate Expectation<Return> Q<S, A>(Random<(S state, A action)> stateActionPair);

    /// <summary>
    /// State vaue function
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public delegate Expectation<Return> V<S>(Random<S> state);
}
