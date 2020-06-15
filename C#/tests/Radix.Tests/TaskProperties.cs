using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async void Property1(NonNegativeInt numberOfCalls)
        {
            int numberOfTries = 0;

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


        [Property(
            DisplayName =
                "When a filter applies, retrying occurs",
            Verbose = true)]
        public async void Property2(NonNegativeInt numberOfCalls)
        {
            int numberOfTries = 0;

            try
            {
                await new Func<Task<int>>(
                        () =>
                        {
                            Interlocked.Increment(ref numberOfTries);
                            return Task.FromException<int>(new ApplicationException());
                        })
                    .Where(exception => exception is ApplicationException)
                    .Retry(Enumerable.Repeat(TimeSpan.FromMilliseconds(1), numberOfCalls.Get).ToArray());
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.Should().Contain(exception => exception is ApplicationException);
                Equal(numberOfCalls.Get + 1, numberOfTries);
            }
        }

        [Property(
            DisplayName =
                "When a filter does not apply, it prevents retrying",
            Verbose = true)]
        public async void Property3(NonNegativeInt numberOfCalls)
        {
            int numberOfTries = 0;

            try
            {
                await new Func<Task<int>>(
                        () =>
                        {
                            Interlocked.Increment(ref numberOfTries);
                            return Task.FromException<int>(new ApplicationException());
                        })
                    .Where(exception => exception is ArgumentNullException)
                    .Retry(Enumerable.Repeat(TimeSpan.FromMilliseconds(1), numberOfCalls.Get).ToArray());
            }
            catch
            {
                Equal(1, numberOfTries);
            }
        }
    }
}
