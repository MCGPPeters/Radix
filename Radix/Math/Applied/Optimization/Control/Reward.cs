using Radix.Generators.Attributes;

namespace Radix.Math.Applied.Optimization.Control;

/// <summary>
///     A reward (at time step t) is a scalar value or feedback signal
///     that indicated how well an agent is doing at time t.
///     
///     It may be better to sacrifice immediate reward 
///     to gain more reward in the future (long term reward).
///     
///     The reward hypothesis is the idea that all goals can be described 
///     by the maximization of expected cumulative reward.	
/// </summary>
[Alias<double>]
public readonly partial record struct Reward;



