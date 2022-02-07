namespace Radix.Math.Pure.Logic.Order.Intervals;

public record Closed<T, TInterval>(T LowerBound, T UpperBound) : Interval<T, TInterval>
    where T : Order<T>
    where TInterval : Interval<T, TInterval>
{
    public Func<T, bool> Builder =>
        x =>
            x >= LowerBound && x <= UpperBound;
}
