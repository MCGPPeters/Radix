using System;
using FsCheck;
using FsCheck.Xunit;
using Radix.Tests.Result;
using Xunit;
using static Radix.Tests.Result.Extensions;
using static Xunit.Assert;
using static Radix.Tests.Assert;

namespace Radix.Tests
{
    public class ResultProperties
    {
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

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test5(int i)
        {
            Func<int, int> id = x => x;
            var result = Ok(i);

            Equal(result, result.Map(id));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Preserve composition of morphisms")]
        public void Test6(int i)
        {
            Func<string, int> f = x => int.Parse(x);
            Func<int, string> g = x => x.ToString();
            var result = Ok(i);

            Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test7(int i)
        {
            Func<int, int> id = x => x;
            var selector = Ok(id);
            var result = Ok(i);

            Equal(selector.Apply(result), result);
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Composition preservation")]
        public void Test8(int i)
        {
            // todo: hard to express in C#
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Homomorphism")]
        public void Test9(int i)
        {
            Func<int, int> f = x => x;
            var selector = Ok(f);
            var result = Ok(i);

            Equal(Ok(f(i)), selector.Apply(result));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Interchange")]
        public void Test10(int i)
        {
            Func<int, int> f = x => x;

            
            var selector = Ok(f);
            var result = Ok(i);

            Equal(selector.Apply(result), Ok(new Func<Func<int, int>, int>(function => function(i))).Apply(selector));
        }
    }

}
