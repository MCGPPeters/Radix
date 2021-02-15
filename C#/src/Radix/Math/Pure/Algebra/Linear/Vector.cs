using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Numbers;
using static Radix.Result.Extensions;

namespace Radix.Math.Pure.Algebra.Linear
{

    public interface Vector<out T>
    {
        T[] Elements { get; }
    }
}
