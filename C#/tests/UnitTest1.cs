using System;
using System.Threading.Tasks;
using FsCheck.Xunit;
using Radix.Tests.Either;
using Xunit;
using static Xunit.Assert;

namespace Radix.Tests
{

    public interface Either<out Left, out Right>
    {
        
    }

    public struct None
    {
        internal static readonly None Create = new None();
    }

    public interface Maybe<T> : Either<T, None>
    {
        
    }

    namespace Either
    {
        public static class Extensions
        {

        }

        public struct Left<L> : Either<L, None>
        {
            private Left(L l) 
            {
                this.Value = l;
            }

            private static Left<L> Create(L l)
            {
                if(l is object) return new Left<L>(l);
                return new Left<L>(None.Create);
            }

            public L Value { get; }
        }

        public struct Otherwise<This, That> : Either<This, That>{}

    }



    public class ValidationProperties
    {
        private static void Fail() => True(false);
        private static void Pass() => True(true);

        [Fact(
            DisplayName =
                "Can pattern match over cases")]
        public async Task Test1()
        {
            Either<string, int> either = "A string";

            switch (either){
                case Just<string, int> just:
                    Pass();
                    break;
                case _:
                    Fail();
                    break;
            }
        }
    }

}
