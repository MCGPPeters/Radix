using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Probability;
using static Radix.Collections.Generic.Enumerable.Extensions;

namespace Radix.Math.Applied.Learning.Reinforced
{
    public static class Select
    {
        public static A Greedy<A>(Q<A> q, IEnumerable<A> actions) => actions.Max(a => (a, q)).a;


        public static A εGreedy<A>(Q<A> q, double ε, IEnumerable<A> actions) where A : notnull
        {
            Random<double>? σ = Distribution<double>.Uniform(Sequence(0.0, 1.0)).Choose();
            return σ > ε
                ? Greedy(q, actions)
                : Distribution<A>.Uniform(actions).Choose();
        }
    }
}
