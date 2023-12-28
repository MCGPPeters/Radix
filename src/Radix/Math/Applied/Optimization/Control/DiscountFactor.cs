using Radix.Data.Double.Validity;
using Radix.Data.Number.Validity;

namespace Radix.Math.Applied.Optimization.Control;

[Validated<double, InUnitInterval>]
public partial record struct DiscountFactor;

/// <summary>
/// Step size or learning rate. Determines to what extent newly acquired information overrides old information.
/// A factor of 0 makes the agent learn nothing (exclusively exploiting prior knowledge), 
/// while a factor of 1 makes the agent consider only the most recent information (ignoring prior knowledge to explore possibilities).
/// Normally indicated by the greek letter alpha (α).
/// </summary>
[Validated<double, InUnitInterval>]
public partial record struct LearningRate;


/// <summary>
/// Exploration rate. Determines how often the agent chooses a random action.
/// A factor of 0 makes the agent always choose the greedy action (exploitation),
/// while a factor of 1 makes the agent always choose a random action (exploration).
/// Normally indicated by the greek letter epsilon (ε).
/// </summary>
[Validated<double, IsGreaterThanZero<double>>]
public readonly partial record struct ExplorationRate;



