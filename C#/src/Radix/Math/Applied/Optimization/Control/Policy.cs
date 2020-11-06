using System.Collections.Generic;
using Radix.Math.Applied.Probability;
using static System.Math;

namespace Radix.Math.Applied.Optimization.Control
{
    // A policy is a mapping from states to the probabilities of selecting each possible action
    public delegate Distribution<A> Policy<S, A>(S state);

    public static class Policy
    {
        public static Expectation<S> Evaluate<S, A>(Policy<S, A> policy, MDP<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull
        {
            var Δ = 0.0;
            do
            {
                Δ = 0.0;
                foreach (var state in mdp.States)
                {
                    var value = stateValues[state];
                    var newValue = Bellman.Expectation(state, mdp, stateValues);
                    stateValues[state] = newValue;
                    Δ = Max(Δ, Abs(value - newValue));
                }
            }
            while (Δ >= θ);
            return new Expectation<S>(s => stateValues[s]);
        }
    }
}
