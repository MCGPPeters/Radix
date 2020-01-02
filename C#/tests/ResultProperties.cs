using FsCheck;
using FsCheck.Xunit;
using Radix.Monoid;
using Radix.Result;
using System;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Tests.Assert;
using static Xunit.Assert;

namespace Radix.Tests
{
    public partial class ResultProperties
    {
        [Property(
            DisplayName =
                "As a developer I want to be able to use the language support for pattern matching, so that the code is easy to understand")]
        public void Test1(NonEmptyString s)
        {
            var result = Ok<MString, MString>(s.Get);

            switch (result)
            {
                case Ok<MString, MString> _:
                    Pass();
                    break;
                case Error<MString, MString> _:
                    Fail();
                    break;
            }
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Left identity law")]
        public void Test2(int i)
        {
            var result = Ok<int, MInt>(i);

            Func<int, Result<int, MInt>> f = Ok<int, MInt>;

            Equal(result.Bind(f), f(i));
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Right identity law")]
        public void Test3(int i)
        {
            var result = Ok<MInt, MInt>(i);

            Equal(result.Bind(Ok<MInt, MInt>), result);
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Associativity law")]
        public void Test4(int i)
        {
            var result = Ok<int, MString>(i);

            Func<int, Result<string, MString>> f = x => Ok<string, MString>(x.ToString());
            Func<string, Result<int, MString>> g = x => Ok<int, MString>(int.Parse(x));

            Equal(result.Bind(f).Bind(g), result.Bind(x => f(x).Bind(g)));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test5(int i)
        {
            Func<MInt, MInt> id = x => x;
            var result = Ok<MInt, MInt>(i);

            Equal(result, result.Map(id));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Preserve composition of morphisms")]
        public void Test6(int i)
        {
            Func<string, int> f = x => int.Parse(x);
            Func<int, string> g = x => x.ToString();
            var result = Ok<int, MString>(i);

            Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test7(int i)
        {
            Func<MInt, MInt> id = x => x;
            var selector = Ok<Func<MInt, MInt>, MInt>(id);
            var result = Ok<MInt, MInt>(i);

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
            var selector = Ok<Func<int, int>, MInt>(f);
            var result = Ok<int, MInt>(i);

            Equal(Ok<int, MInt>(f(i)), selector.Apply(result));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Interchange")]
        public void Test10(int i)
        {
            Func<int, int> f = x => x;


            var selector = Ok<Func<int, int>, MInt>(f);
            var result = Ok<int, MInt>(i);

            Equal(selector.Apply(result), Ok<Func<Func<int, int>, int>, MInt>(function => function(i)).Apply(selector));
        }

    }

}
