namespace Radix.Math.Pure.Analysis.Measure;

public interface Dimension<out T>
    where T : Unit<T>, Literal<T>
{
    Numbers.Number Quantity { get; init; }
};




