using Radix.Math.Pure.Analysis.Measure;
using Radix.Math.Pure.Numbers;

namespace Radix.Physics;

public interface Quantity<out T>
    where T : Unit<T>, Literal<T>
{
    Number Value { get; init; }
};




