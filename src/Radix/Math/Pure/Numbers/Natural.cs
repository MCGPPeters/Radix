using Radix.Math.Pure.Numbers.ℕ;

namespace Radix.Math.Pure.Numbers;

public abstract record Natural : Number
{
    public static Natural operator +(Natural x, Natural y) => Addition.Combine(x, y);

    public static Natural operator *(Natural x, Natural y) => Multiplication.Combine(x, y);
}

public record Zero : Natural;

public record Successor(Natural Natural) : Natural;
