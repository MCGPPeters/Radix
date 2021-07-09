using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Radix.Math.Applied.Probability;
using Radix.Math.Applied.Probability.Distribution;
using Radix.Option;
using static System.Math;
using static Radix.Math.Applied.Probability.Distribution.Generators;

namespace Radix.Math.Applied.Optimization.Control.Deterministic
{
    public static class PolicyExtensions
    {
        /// <summary>
        /// Test how good a policy is. Do this by, for each state in the state space, calculating the value of being in that space when following policy π
        /// In each state, for each action:
        /// - calculate how much reward you would get when following that action
        /// - then get the probability that state transition actually occurs and weigh it by the probability the agent actually takes the action (given that it still can taken each action given its probability)
        /// - use that probability to calculate the expected return when taking the action in this state under policy π
        /// - add the return to the total value of state S (which is the maximum reward one could get in this state following policy π from this state onward)
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="mdp"></param>
        /// <param name="stateValues"></param>
        /// <param name="θ"></param>
        /// <returns></returns>
        public static Dictionary<S, double> Evaluate<S, A>(Dictionary<S, Distribution<A>> π, MDP<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull where A : notnull
        {
            double Δ = 0.0;
            do
            {
                Δ = 0.0;
                foreach (S state in mdp.States)
                {
                    double oldValue = stateValues[state];
                    double value = 0.0;

                    foreach (A action in mdp.Actions)
                    {
                        (Transition<S> transition, Reward reward, Option<Probability.Probability> optionalProbability) = mdp.Dynamics(state, action);
                        Probability.Probability? actionProbability = π[state].Value.First(eventProbability => eventProbability.@event.Value.Equals(action)).probability;

                        value += optionalProbability switch
                        {
                            Some<Probability.Probability> transitionProbability => actionProbability * transitionProbability.Value * (reward + mdp.DiscountFactor * stateValues[transition.Destination]),
                            _ => 1.0 * (reward + mdp.DiscountFactor * stateValues[transition.Destination])
                        };
                    }

                    stateValues[state] = value;
                    Δ = Max(Δ, Abs(oldValue - value));
                }
            } while (Δ >= θ);

            return stateValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="π"></param>
        /// <param name="mdp"></param>
        /// <param name="stateValues"></param>
        /// <param name="θ"></param>
        /// <returns></returns>
        public static Dictionary<S, Distribution<A>> Improve<S, A>(Dictionary<S, Distribution<A>> π, MDP<S, A> mdp, Dictionary<S, double> stateValues) where S : notnull where A : notnull
        {
            foreach (var state in mdp.States)
            {
                // select the actions actually yielding the most reward as a distribution
                var bestActions = mdp.Actions.ArgMax(
                    action =>
                    {
                        (Transition<S> transition, Reward reward, Option<Probability.Probability> optionalProbability) = mdp.Dynamics(state, action);
                        return optionalProbability switch
                        {
                            Some<Probability.Probability> transitionProbability =>
                                transitionProbability.Value * (reward + mdp.DiscountFactor * stateValues[transition.Destination]),
                            _ => 1.0 * (reward + mdp.DiscountFactor * stateValues[transition.Destination])
                        };
                    });
                π[state] = Distribution<A>.Uniform(bestActions);
            }

            return new Dictionary<S, Distribution<A>>(π);
        }

        public static Dictionary<S, Distribution<A>> Iterate<S, A>(Dictionary<S, Distribution<A>> π, MDP<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull where A : notnull
        {
            var isPolicyStable = false;
            var currentPolicy = π;


            while (!isPolicyStable)
            {
                Evaluate(currentPolicy, mdp, stateValues, 0.001);

                var newPolicy = Improve(currentPolicy, mdp, stateValues);

                isPolicyStable = newPolicy == currentPolicy;

                currentPolicy = newPolicy;
            }


            return currentPolicy;
        }
    }
}
