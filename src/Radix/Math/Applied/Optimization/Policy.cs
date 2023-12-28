using Radix.Math.Applied.Learning.Reinforced.TemporalDifference.Control.OffPolicy;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;
using static Radix.Math.Applied.Probability.Distribution.Generators;
using static Radix.Data.Collections.Generic.Enumerable.Extensions;

namespace Radix.Math.Applied.Optimization;

/// <summary>
///     A stochastic policy is a mapping from states to the probabilities of selecting each possible action
/// </summary>
public static class Policy
{
    /// <summary>
    ///    The greedy policy is the policy that always chooses the greedy action.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="q"></param>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static Policy<S, A> Greedy<S, A>(Q<S, A> q, params A[] actions) => 
        s =>
            Certainly(actions.MaxBy(a => q(s)(a), new ExpectationComparer()))!;

    /// <summary>
    ///    The ε-greedy policy is the policy that chooses the greedy action with probability 1 − ε and a random action with probability ε.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="q"></param>
    /// <param name="ε"></param>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static Policy<S, A> εGreedy<S, A>(Q<S, A> q, double ε, A[] actions) =>
        s => 
        {
            Random<double>? σ = Distribution<double>.Uniform(Sequence(0.0, 1.0)).Choose();
            return σ > ε
                ? Greedy(q, actions)(s)
                : Distribution<A>.Uniform(actions);
        };
}

/// <summary>
///     A stochastic policy is a mapping from states to the probabilities of selecting each possible action
///     
/// </summary>
/// <typeparam name="S"></typeparam>
/// <typeparam name="A"></typeparam>
/// <param name="state"></param>
/// <returns></returns>
public delegate Distribution<A> Policy<in S, A>(S state);
