using FsCheck.Xunit;
using Radix.Control.Result;
using Radix.Data;
using Xunit;

using static Radix.Control.Result.Extensions;
using static Xunit.Assert;

namespace Radix.Tests
{
    public class ResultProperties
    {
        [Property(
            DisplayName =
                "As a developer I want to be able to use the language support for pattern matching, so that the code is easy to understand")]
        public void Test1(FsCheck.NonEmptyString s)
        {
            Result<string, string> result = Ok<string, string>(s.Get);

            switch (result)
            {
                case Ok<string, string> _:
                    Pass();
                    break;
                case Error<string, string> _:
                    Fail();
                    break;
            }
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Left identity law")]
        public void Test2(int i)
        {
            Result<int, int> result = Ok<int, int>(i);

            Func<int, Result<int, int>> f = Ok<int, int>;

            Equal(result.Bind(f), f(i));
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Right identity law")]
        public void Test3(int i)
        {
            Result<int, int> result = Ok<int, int>(i);

            Equal(result.Bind(Ok<int, int>), result);
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Associativity law")]
        public void Test4(int i)
        {
            Result<int, string> result = Ok<int, string>(i);

            Func<int, Result<string, string>> f = x => Ok<string, string>(x.ToString());
            Func<string, Result<int, string>> g = x => Ok<int, string>(int.Parse(x));

            Equal(result.Bind(f).Bind(g), result.Bind(x => f(x).Bind(g)));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test5(int i)
        {
            Func<int, int> id = x => x;
            Result<int, int> result = Ok<int, int>(i);

            Equal(result, result.Map(id));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Preserve composition of morphisms")]
        public void Test6(int i)
        {
            Func<string, int> f = x => int.Parse(x);
            Func<int, string> g = x => x.ToString();
            Result<int, string> result = Ok<int, string>(i);

            Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test7(int i)
        {
            Func<int, int> id = x => x;
            Result<Func<int, int>, int> selector = Ok<Func<int, int>, int>(id);
            Result<int, int> result = Ok<int, int>(i);

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
            Result<Func<int, int>, int> selector = Ok<Func<int, int>, int>(f);
            Result<int, int> result = Ok<int, int>(i);

            Equal(Ok<int, int>(f(i)), selector.Apply(result));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Interchange")]
        public void Test10(int i)
        {
            Func<int, int> f = x => x;


            Result<Func<int, int>, int> selector = Ok<Func<int, int>, int>(f);
            Result<int, int> result = Ok<int, int>(i);

            Equal(selector.Apply(result), Ok<Func<Func<int, int>, int>, int>(function => function(i)).Apply(selector));
        }
    }

}
