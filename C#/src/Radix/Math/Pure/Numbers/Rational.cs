using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;
using static Radix.Math.Pure.Numbers.ℤ.Extensions;

namespace Radix.Math.Pure.Numbers
{
    using static Result.Extensions;
    public record Rational : Field<Rational>
    {
        public Integer Numerator { get; }
        public Integer Denominator { get; }

        private Rational(Integer numerator, Integer denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static Rational operator *(Rational x, Rational y) =>
            new (x.Numerator * y.Numerator, x.Denominator * y.Denominator);

        public static Rational operator -(Rational x, Rational y)
        {
            switch (x.Denominator == y.Denominator)
            {
                case true:
                    return new Rational(x.Numerator + y.Numerator, x.Denominator);
                case false:
                {
                    var lcm = Lcm(x.Denominator, y.Denominator);
                    var xNumerator = ((lcm / x.Denominator) * x.Numerator);
                    var yNumerator = ((lcm / y.Denominator) * y.Numerator);

                    return new Rational(xNumerator - yNumerator, x.Denominator);
                }
            };
        }

        public static Rational operator /(Rational x, Rational y)
        {
            var switched = new Rational(y.Denominator, y.Numerator);
            return x * switched;
        }

        public static Rational operator +(Rational x, Rational y)
        {
            switch (x.Denominator == y.Denominator)
            {
                case true:
                    return new Rational(x.Numerator + y.Numerator, x.Denominator);
                case false:
                {
                    var lcm = Lcm(x.Denominator, y.Denominator);
                    var xNumerator = ((lcm / x.Denominator) * x.Numerator);
                    var yNumerator = ((lcm / y.Denominator) * y.Numerator);

                    return new Rational(xNumerator + yNumerator, x.Denominator);
                }
            };
        }

        public static Rational operator -(Rational x) =>
            new Rational(0, 1) - x;

        public static Result<Rational, Error> Create(Integer numerator, Integer denominator)
            => denominator.Value switch
            {
                > 0 => Ok<Rational, Error>(new Rational(numerator, denominator)),
                _ => Error<Rational, Error>("The denominator must be greater then 0")
            };

        Multiplication<Rational> Semigroup<Rational, Multiplication<Rational>>.Combine
        {
            get => new((x, y) => x * y);
        }

        Rational Monoid<Rational, Addition<Rational>>.Identity
        {
            get => new(0, 1);
        }

        Addition<Rational> Semigroup<Rational, Addition<Rational>>.Combine
        {
            get => new((x, y) => x + y);
        }

        Rational Group<Rational, Addition<Rational>>.Invert() => -this;

        Rational Monoid<Rational, Multiplication<Rational>>.Identity
        {
            get => new(1, 1);
        }
    }
}
