using System.Collections.Generic;
using Radix.Math.Applied.Optimization.Control.Stochastic;
using Radix.Math.Applied.Probability;
using static System.Math;

namespace Radix.Math.Applied.Optimization.Control.Deterministic
{
    public record Policy<S, A>() : Alias<Dictionary<S, A>>(new Dictionary<S, A>()) where S : notnull;

    public static class PolicyExtensions
    {
        public static Expectation<S> Evaluate<S, A>(this Policy<S, A> π, MDP<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull
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


        //public static Policy<S, A> Improve<S, A>(this Policy<S, A> π, MDP<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull
        //{

        //}

        //public static Policy<S, A> Iterate<S, A>()
        //{
        //    bool policyIsInstable = false;

        //    while (policyIsInstable)
        //    {
        //        var expectedValue = Evaluate(π, mdp, stateValues, 0.001);

        //        foreach (var state in mdp.States)
        //        {
        //            var action = π(state);


        //        }
        //    }
        //}
    }
}
