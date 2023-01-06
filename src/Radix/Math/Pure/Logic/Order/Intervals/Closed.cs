﻿namespace Radix.Math.Pure.Logic.Order.Intervals;

public record Closed<T>(T LowerBound, T UpperBound) : Interval<T>
    where T : Order<T>
{
    public Func<T, bool> Contains =>
        x =>
            x >= LowerBound && x <= UpperBound;
}
