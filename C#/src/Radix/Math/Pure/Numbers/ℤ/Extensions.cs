﻿using System;

namespace Radix.Math.Pure.Numbers.ℤ
{
    public class Extensions
    {
        public static Integer Lcm(Integer x, Integer y)
        {
            int absoluteX = System.Math.Abs(x);
            int absoluteY = System.Math.Abs(y);

            return (absoluteX / Gcd(x, y) * absoluteY);
        }

        private static Func<Integer, Integer, Integer> Gcd =>
            (x, y) =>
            {
                static Integer Gcd(Integer x, Integer y)
                    => (x.Value, y.Value) switch
                    {
                        (0, _) => x,
                        _ => Gcd(x, x % y)
                    };

                return Gcd(x, y);
            };
    }
}
