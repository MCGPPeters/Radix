namespace Radix.Math.Pure.Numbers.ℤ;

public static class Extensions
{

    public static Func<Integer, Integer, Integer> Gcd =>
        (x, y) =>
        {
            static Integer Gcd(Integer x, Integer y) => (x.Value, y.Value) switch
            {
                (0, _) => x,
                _ => Gcd(x, x % y)
            };

            return Gcd(x, y);
        };

    public static Func<Integer, Integer, Integer> Lcm =>
        (x, y) =>
        {
            int absoluteX = System.Math.Abs(x);
            int absoluteY = System.Math.Abs(y);

            return absoluteX / Gcd(x, y) * absoluteY;
        };
}
