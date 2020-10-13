﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Collections.Generic.Enumerable;
using Radix.Math.Applied.Optimization;
using Radix.Math.Applied.Probability;
using static Radix.Math.Applied.Probability.Sampling;
using static Radix.Collections.Generic.Enumerable.Extensions;

namespace Radix.Math.Applied.Learning.Reinforced
{
    public static class Select
    {
        public static A Greedy<A>(Q<A> q, IEnumerable<A> actions) => actions.Max(a => (a, q.Value(a))).a;


        public static A εGreedy<A>(Q<A> q, double ε,  IEnumerable<A> actions)
        {
            var σ = Distribution<double>.Uniform(Sequence(0.0, 1.0)).Choose();
            return (σ > ε)
                ? Greedy(q, actions)
                : Distribution<A>.Uniform(actions).Choose();
        }
    }
}
