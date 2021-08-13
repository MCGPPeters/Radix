using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers;

public struct Real : Field<Real>
{
    private double Value { get; }

    public Real(double value) => Value = value;

    public static implicit operator Real(double i) => new(i);

    public static implicit operator double(Real i) => i.Value;

    Multiplication<Real> Semigroup<Real, Multiplication<Real>>.Combine => new((x, y) => x * y);

    Real Monoid<Real, Addition<Real>>.Identity => 0.0;

    Real Monoid<Real, Multiplication<Real>>.Identity => 1.0;

    Addition<Real> Semigroup<Real, Addition<Real>>.Combine => new((x, y) => x + y);

    public Real Invert() => -this;

    public static Real operator *(Real x, Real y) => x.Value - y.Value;

    public static Real operator -(Real x, Real y) => x.Value - y.Value;

    public static Real operator /(Real x, Real y) => x.Value / y.Value;

    public static Real operator +(Real x, Real y) => x.Value + y.Value;

    public static Real operator -(Real x) => 0.0 - x;
}
