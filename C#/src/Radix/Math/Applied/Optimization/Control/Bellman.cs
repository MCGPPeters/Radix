using System.Collections.Generic;
using Radix.Option;

namespace Radix.Math.Applied.Optimization.Control
{
    public static class Bellman
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="policy"></param>
        /// <param name="mdp"></param>
        /// <param name="vπ"></param>
        /// <returns></returns>
        public static double Expectation<S, A>(S state, MDP<S, A> mdp, Dictionary<S, double> stateValues) where S : notnull
        {
            double value = 0.0;

            foreach (A action in mdp.Actions)
            {
                (Transition<S> transition, Reward reward, Option<Probability.Probability> optionalProbability) result = mdp.Dynamics(state, action);

                value += result.optionalProbability switch
                {
                    Some<Probability.Probability> probability => probability.Value * (result.reward + mdp.DiscountFactor * stateValues[result.transition.Destination]),
                    _ => 1.0 * (result.reward + mdp.DiscountFactor * stateValues[result.transition.Destination])
                };
            }

            return value;
        }
    }
}
