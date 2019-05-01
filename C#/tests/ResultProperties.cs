using System;
using FsCheck;
using FsCheck.Xunit;
using Radix.Tests.Result;
using Xunit;
using static Radix.Tests.Result.Extensions;
using static Xunit.Assert;

namespace Radix.Tests
{


    public class ResultProperties
    {
        private static void Fail()
        {
            True(false);
        }

        private static void Pass()
        {
            True(true);
        }

        [Property(
            DisplayName =
                "As a developer I want to be able to use the language support for pattern matching, so that the code is easy to understand")]
        public void Test1(NonEmptyString s)
        {
            var result = Ok(s.Get);

            switch (result)
            {
                case Ok<string> _:
                    Pass();
                    break;
                case Error<string> _:
                    Fail();
                    break;
            }
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Left identity law")]
        public void Test2(int i)
        {
            var result = Ok(i);

            Func<int, Result<int>> f = Ok;

            Equal(result.Bind(f), f(i));
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Right identity law")]
        public void Test3(int i)
        {
            var result = Ok(i);

            Equal(result.Bind(x => Ok(x)), result);
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Associativity law")]
        public void Test4(int i)
        {
            var result = Ok(i);

            Func<int, Result<string>> f = x => Ok(x.ToString());
            Func<string, Result<int>> g = x => Ok(int.Parse(x));

            Equal(result.Bind(f).Bind(g), result.Bind(x => f(x).Bind(g)));
        }
    }

}
