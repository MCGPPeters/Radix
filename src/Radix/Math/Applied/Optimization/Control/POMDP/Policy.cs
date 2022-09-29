using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Optimization.Control.POMDP;

/// <summary>
/// A stochastic policy is a mapping from states to the probabilities of selecting each possible action
/// </summary>

/// <returns></returns>
public delegate Distribution<A> Policy<in O, A>(O observation) where A : notnull;
