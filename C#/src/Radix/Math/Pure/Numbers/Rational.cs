namespace Radix.Math.Pure.Numbers
{
    using static Result.Extensions;
    public record Rational
    {
        public int Numerator { get; }
        public int Denominator { get; }

        private Rational(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static Result<Rational, Error> Create(int numerator, int denominator)
            => denominator switch
            {
                > 0 => Ok<Rational, Error>(new Rational(numerator, denominator)),
                _ => Error<Rational, Error>("The denominator must be greater then 0")
            };


    }
}
