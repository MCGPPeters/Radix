using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data;

public record struct Writer<T, TOutput>(T value, TOutput output) where TOutput : Monoid<TOutput>;
