using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using static Radix.Async.Extensions;
using static Xunit.Assert;

namespace Radix.Tests
{

    public class TaskProperties
    {
        [Property(
            DisplayName =
                "Retries equal the number of delays",
            Verbose = true)]
        public async void Property2(NonNegativeInt numberOfCalls)
        {
            var numberOfTries = 0;

            try
            {
                await new Func<Task<int>>(
                    () =>
                    {
                        Interlocked.Increment(ref numberOfTries);
                        return Task.FromException<int>(new Exception());
                    }).Retry(Enumerable.Repeat(TimeSpan.FromMilliseconds(1), numberOfCalls.Get).ToArray());
            }
            catch
            {
                Equal(numberOfCalls.Get + 1, numberOfTries);
            }

        }
    }
}
