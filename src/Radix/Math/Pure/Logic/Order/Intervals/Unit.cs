using Radix.Math.Pure.Numbers;

namespace Radix.Math.Pure.Logic.Order.Intervals;

public record Unit : Closed<Real, Unit>
{
    private static readonly Unit _instance = new Unit();

    private Unit() : base((Real)0.0,(Real)1.0)
    {
    }

    public static Unit Instance => _instance;
}
