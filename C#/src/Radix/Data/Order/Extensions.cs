namespace Radix.Data.Order;

public static class Extensions
{
    public static bool LessThan<T, TOrd>(this T x, T y)
        where TOrd : Order<T> =>
            TOrd.Compare(x, y) switch
            {
                LT => true,
                _ => false
            };


    public static bool GreaterThan<T, TOrd>(this T x, T y)
        where TOrd : Order<T> =>
            TOrd.Compare(x, y) switch
            {
                GT => true,
                _ => false
            };

    public static bool Equals<T, TOrd>(this T x, T y)
       where TOrd : Order<T> =>
       TOrd.Compare(x, y) switch
       {
           EQ => true,
           _ => false
       };

    public static bool NotEquals<T, TOrd>(this T x, T y)
       where TOrd : Order<T> =>
            !x.Equals(y);
}
