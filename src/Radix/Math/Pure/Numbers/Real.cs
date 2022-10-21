namespace Radix.Math.Pure.Numbers;

[Alias<double>]
public partial struct Real : Order<Real>, Number
{
    public static Func<Real, Real, Ordering> Compare => (x, y) =>
        Comparer<double>.Default.Compare(x, y) switch
        {
            < 0 => new LT(),
            0 => new EQ(),
            _ => new GT()
        };

    public static Func<Real, Real, bool> Equal => (x, y)
        => Compare(x, y) == new EQ();

    public static Func<Real, Real, bool> NotEqual => (x, y)
        => !Equal(x, y);

    public static bool operator <(Real left, Real right) =>
        Compare(left, right) == new LT();

    public static bool operator >(Real left, Real right) =>
        Compare(left, right) == new GT();

    public static bool operator <=(Real left, Real right) =>
        left < right || Equal(left, right);
    public static bool operator >=(Real left, Real right) =>
        left > right || Equal(left, right);
}
