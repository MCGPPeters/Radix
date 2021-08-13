namespace Radix.Math.Applied.Probability;

public delegate double Expectation<T>(Random<T> Value) where T : notnull;
