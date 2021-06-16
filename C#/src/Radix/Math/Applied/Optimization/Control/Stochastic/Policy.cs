using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Optimization.Control.Stochastic
{
    // A policy is a mapping from states to the probabilities of selecting each possible action
    public delegate Distribution<A> Policy<in S, A>(S state) where A : notnull;
}
