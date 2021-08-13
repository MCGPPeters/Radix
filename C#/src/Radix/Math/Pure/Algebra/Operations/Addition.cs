namespace Radix.Math.Pure.Algebra.Operations;

public class Addition<T> : Binary<T>
{
    public Addition(Func<T, T, T> apply) => Apply = apply;

    public Func<T, T, T> Apply { get; }
}
