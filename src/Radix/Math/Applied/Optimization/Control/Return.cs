namespace Radix.Math.Applied.Optimization.Control;

[Alias<double>]
public partial record struct Return : IComparable
{
    public int CompareTo(object? obj) => Value.CompareTo(obj);
}
