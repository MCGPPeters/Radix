using System;
using System.Linq;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Radix.Tests.Result;
using static Radix.Tests.Result.Extensions;
using Xunit;
using static Xunit.Assert;

namespace Radix.Tests
{
    public struct Unit : IComparable<Unit>, IEquatable<Unit>
    {
        public static readonly Unit Instance = new Unit();

        public int CompareTo(Unit other) => 0;

        public bool Equals(Unit other) => true;

        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode() => 0;

        public override string ToString() => "{}";
    }

    public interface Result<out T>
    {
        
    }

    public interface IMonoid<T>
    {
        T Identity { get; }

        T Append(T t);

        T Concat(IMonoid<T> other);
    }

    public abstract class Monoid<T> : IMonoid<T>
    {
        public abstract T Identity { get; }

        public abstract T Append(T t);

        public T Concat(IMonoid<T> other) => Append(other.Identity);

        public static T operator +(Monoid<T> first, Monoid<T> second) => first.Concat(second);
    }

    

    

    namespace Result
    {
        public static class Extensions
        {
            public static Result<T> Ok<T>(T t) => new Ok<T>(t);          
            public static Result<T> Bind<T, TResult>(
            this Result<T> result,
            Func<T, Result<T>> function) 
            {
                switch (result)
                {
                    case Ok<T> ok:
                        return function(ok);
                    case Error<T> error:
                        return error;
                }
            } 
        }

        public class Error<T> : Monoid<Error<T>>, Result<T>
        {
            private Error(params string[] messages) => Messages = messages;

            public string[] Messages { get; }

            public override Error<T> Identity => Messages;

            public static implicit operator Error<T>(string message) => new Error<T>(message);

            public static implicit operator Error<T>(string[] messages) => messages.Select(m => new Error<T>(m))
                .Aggregate((current, next) => current.Append(next));

            public static implicit operator string[](Error<T> error) => error.Messages;

            public static implicit operator string (Error<T> error) => error.Messages.Select(m => new Error<T>(m))
                .Aggregate((current, next) => current.Append(next));

            public override Error<T> Append(Error<T> t) => new Error<T>(Messages.Concat(t.Messages).ToArray());

            public void Deconstruct(out string[] messages)
            {
                messages = Messages;
            }
        }

        public struct Ok<T> : Result<T>
        {
            internal Ok(T t) 
            {
                if(t is object) this.Value = t;
                else
                    throw new ArgumentNullException(nameof(t));
                
            }

            public static implicit operator Ok<T>(T t) => new Ok<T>(t);
            public static implicit operator T(Ok<T> ok) => ok.Value;

            public T Value { get; }
        }

    }



    public class EitherProperties
    {
        private static void Fail() => True(false);
        private static void Pass() => True(true);

        [Property(
            DisplayName =
                "Can pattern match over cases")]
        public void Test1(NonEmptyString s)
        {
            var either = Ok(s.Get);

            switch (either){
                case Ok<string> _:
                    Pass();
                    break;
                case Error<string> _:
                    Fail();
                    break;
            }
        }
    }

}
