using System;
using FsCheck;
using FsCheck.Xunit;
using Radix.Data;
using Radix.Math.Pure.Numbers;
using Radix.Result;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Tests.Assert;
using static Xunit.Assert;
using String = Radix.Data.String;

namespace Radix.Tests
{
    public class ResultProperties
    {
        [Property(
            DisplayName =
                "As a developer I want to be able to use the language support for pattern matching, so that the code is easy to understand")]
        public void Test1(NonEmptyString s)
        {
            Result<String, String> result = Ok<String, String>(s.Get);

            switch (result)
            {
                case Ok<String, String> _:
                    Pass();
                    break;
                case Error<String, String> _:
                    Fail();
                    break;
            }
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Left identity law")]
        public void Test2(int i)
        {
            Result<int, Integer> result = Ok<int, Integer>(i);

            Func<int, Result<int, Integer>> f = Ok<int, Integer>;

            Equal(result.Bind(f), f(i));
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Right identity law")]
        public void Test3(int i)
        {
            Result<Integer, Integer> result = Ok<Integer, Integer>(i);

            Equal(result.Bind(Ok<Integer, Integer>), result);
        }

        [Trait("Category", "Monad")]
        [Property(DisplayName = "Associativity law")]
        public void Test4(int i)
        {
            Result<int, String> result = Ok<int, String>(i);

            Func<int, Result<string, String>> f = x => Ok<string, String>(x.ToString());
            Func<string, Result<int, String>> g = x => Ok<int, String>(int.Parse(x));

            Equal(result.Bind(f).Bind(g), result.Bind(x => f(x).Bind(g)));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test5(int i)
        {
            Func<Integer, Integer> id = x => x;
            Result<Integer, Integer> result = Ok<Integer, Integer>(i);

            Equal(result, result.Map(id));
        }

        [Trait("Category", "Functor")]
        [Property(DisplayName = "Preserve composition of morphisms")]
        public void Test6(int i)
        {
            Func<string, int> f = x => int.Parse(x);
            Func<int, string> g = x => x.ToString();
            Result<int, String> result = Ok<int, String>(i);

            Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Must preserve identity morphisms")]
        public void Test7(int i)
        {
            Func<Integer, Integer> id = x => x;
            Result<Func<Integer, Integer>, Integer> selector = Ok<Func<Integer, Integer>, Integer>(id);
            Result<Integer, Integer> result = Ok<Integer, Integer>(i);

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
            Result<Func<int, int>, Integer> selector = Ok<Func<int, int>, Integer>(f);
            Result<int, Integer> result = Ok<int, Integer>(i);

            Equal(Ok<int, Integer>(f(i)), selector.Apply(result));
        }

        [Trait("Category", "Applicative functor")]
        [Property(DisplayName = "Interchange")]
        public void Test10(int i)
        {
            Func<int, int> f = x => x;


            Result<Func<int, int>, Integer> selector = Ok<Func<int, int>, Integer>(f);
            Result<int, Integer> result = Ok<int, Integer>(i);

            Equal(selector.Apply(result), Ok<Func<Func<int, int>, int>, Integer>(function => function(i)).Apply(selector));
        }
    }

}
