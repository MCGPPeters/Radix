using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Optimization.Control;

/// <summary>
///     The action value function for a policy π is the expected return when starting in s, taking action a, and thereafter following π.
/// </summary>
/// <typeparam name="S"></typeparam>
/// <typeparam name="A"></typeparam>
/// <param name="state"></param>
/// <returns></returns>
public delegate Func<A, Expectation<Return>> Q<in S, in A>(S state);

