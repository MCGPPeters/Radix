using System;
using Radix.Math.Pure.Logic.Order.Intervals;
using Radix.Math.Pure.Numbers;
using static Radix.Control.Validated.Extensions;

namespace Radix.Data.String.Validity;


public record LengthIsInClosedInterval : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
       name =>
            value =>
                new Closed<Integer>(LowerBound, UpperBound).Contains((Integer)value.Length)
                    ? Valid(value)
                    : Invalid<string>(name, $"The minimul length of '{name}' must be greater or equal to {LowerBound} and smaller or equal to {UpperBound}");

  

    public static Integer LowerBound { get; set; }
    public static Integer UpperBound { get; set; }

}
