namespace Radix.Math.Pure.Logic.Order;

public interface Interval<T, TInterval> : Set<T, TInterval>
    where T : Order<T>
    where TInterval : Set<T, TInterval>
{
}
