using System.Collections.ObjectModel;
using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Algebra.Linear;

public interface Vector<T, FAdd, FMul>
    where FAdd : Field<T>, Addition
    where FMul : Field<T>, Multiplication
{
    ReadOnlyCollection<T> Elements { get; }
}
