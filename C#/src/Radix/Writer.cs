using Radix.Math.Pure.Algebra.Structure;

namespace Radix;

public record struct Writer<T, TOutput, Monoid>(T value, TOutput output) where Monoid : Monoid<TOutput>;
