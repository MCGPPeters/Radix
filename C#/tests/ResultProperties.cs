using System;
using FsCheck;
using FsCheck.Xunit;
using Radix.Tests.Monoid;
using Radix.Tests.Result;
using Xunit;
using static Radix.Tests.Result.Extensions;
using static Xunit.Assert;
using static Radix.Tests.Assert;

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
        public void Test2(MInt i)
        {
            var result = Ok<MInt, MInt>(i);

            Func<MInt, Result<MInt, MInt>> f = Ok<MInt, MInt>;

            Equal(result.Bind(f), f(i));
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Right identity law")]
        public void Test3(MInt i)
        {
            var result = Ok<MInt, MInt>(i);

            Equal(result.Bind(x => Ok<MInt, MInt>(x)), result);
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Associativity law")]
        public void Test4(MInt i)
        {
            var result = Ok<MInt, MString>(i);

            Func<MInt, Result<MString, MString>> f = x => Ok<MString, MString>(x.ToString());
            Func<MString, Result<MInt, MString>> g = x => Ok<MInt, MString>(int.Parse(x));

            Equal(result.Bind(f).Bind(g), result.Bind(x => f(x).Bind(g)));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test5(MInt i)
        {
            Func<MInt, MInt> id = x => x;
            var result = Ok<MInt, MInt>(i);

            Equal(result, result.Map(id));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Preserve composition of morphisms")]
        public void Test6(MInt i)
        {
            Func<MString, MInt> f = x => int.Parse(x);
            Func<MInt, MString> g = x => x.ToString();
            var result = Ok<MInt, MString>(i);

            Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test7(MInt i)
        {
            Func<MInt, MInt> id = x => x;
            var selector = Ok<Func<MInt, MInt>, MInt>(id);
            var result = Ok<MInt, MInt>(i);

            Equal(selector.Apply(result), result);
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Composition preservation")]
        public void Test8(MInt i)
        {
            // todo: hard to express in C#
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Homomorphism")]
        public void Test9(MInt i)
        {
            Func<MInt, MInt> f = x => x;
            var selector = Ok<Func<MInt, MInt>, MInt>(f);
            var result = Ok<MInt, MInt>(i);

            Equal(Ok<MInt, MInt>(f(i)), selector.Apply(result));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Interchange")]
        public void Test10(MInt i)
        {
            Func<MInt, MInt> f = x => x;


            var selector = Ok<Func<MInt, MInt>, MInt>(f);
            var result = Ok<MInt, MInt>(i);

            Equal(selector.Apply(result), Ok<Func<Func<MInt, MInt>, MInt>, MInt>(function => function(i)).Apply(selector));
        }

    }

}
