using System;
using System.Linq;

namespace Radix
{
    public static class Backoff
    {

        public static TimeSpan[] Exponentially(int maxNumberOfTimes = 3)
        {
            return Enumerable
                .Range(1, maxNumberOfTimes)
                .Select(
                    x =>
                    {
                        var rnd = new Random();
                        var seconds = TimeSpan.FromSeconds(Math.Pow(2, x));
                        // add random number of milliseconds to decrease the chance globally synchronized retries
                        var milliseconds = TimeSpan.FromMilliseconds(rnd.Next());

                        return seconds.Add(milliseconds);

                    }).ToArray();
        }
    }
}
