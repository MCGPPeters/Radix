namespace Radix.Math.Pure.Numbers.ℤ;

public static class Extensions
{

    public static Func<int, int, int> Gcd =>
        (x, y) =>
        {
            static int Gcd(int x, int y) => (x, y) switch
            {
                (0, _) => x,
                _ => Gcd(x, x % y)
            };

            return Gcd(x, y);
        };

    public static Func<int, int, int> Lcm =>
        (x, y) =>
        {
            int absoluteX = System.Math.Abs(x);
            int absoluteY = System.Math.Abs(y);

            return absoluteX / Gcd(x, y) * absoluteY;
        };
}
