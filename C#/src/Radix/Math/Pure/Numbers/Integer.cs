using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers
{
    public struct Integer : Ring<Integer>
    {

        public int Value { get; private init; }

        Multiplication<Integer> Semigroup<Integer, Multiplication<Integer>>.Combine =>
            new((x, y) => x * y);

        Integer Monoid<Integer, Addition<Integer>>.Identity => 0;
        Integer Monoid<Integer, Multiplication<Integer>>.Identity => 1;

        Addition<Integer> Semigroup<Integer, Addition<Integer>>.Combine =>
            new((x, y) => x + y);

        Integer Group<Integer, Addition<Integer>>.Invert() => -this;

        public static implicit operator Integer(int i) => new() {Value = i};

        public static implicit operator int(Integer i) => i.Value;

        public static Integer operator +(Integer x, Integer y) => x.Value + y.Value;

        public static Integer operator -(Integer x, Integer y) => x.Value - y.Value;

        public static Integer operator *(Integer x, Integer y) => x.Value * y.Value;

        public static Integer operator /(Integer x, Integer y) => x.Value / y.Value;

        public static Integer operator -(Integer x) => 0 - x.Value;

        public static bool operator >(Integer x, Integer y) => x.Value > y.Value;

        public static bool operator <(Integer x, Integer y) => x.Value < y.Value;
    }
}
