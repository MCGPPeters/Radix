using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;
using static Radix.Math.Pure.Numbers.ℕ.Extensions;

namespace Radix.Math.Pure.Numbers;

public abstract record Natural :
    Monoid<Natural, Multiplication<Natural>>,
    Monoid<Natural, Addition<Natural>>
{

    public Addition<Natural> Combine =>
        new((x, y) => x + y);

    public Natural Identity => new Zero();

    Multiplication<Natural> Semigroup<Natural, Multiplication<Natural>>.Combine =>
        new((x, y) => x * y);

    Natural Monoid<Natural, Multiplication<Natural>>.Identity => new Successor(new Zero());

    public static Natural operator +(Natural x, Natural y) => Add.Apply(x, y);

    public static Natural operator *(Natural x, Natural y) => Multiply.Apply(x, y);
}

public record Zero : Natural;

public record Successor(Natural Natural) : Natural;
