using System;

namespace Radix.Validated
{

    public record Valid<T>(T Value) : Validated<T>
    {
        public static implicit operator Valid<T>(T t) => new Valid<T>(t);

        public static implicit operator T(Valid<T> ok) => ok.Value;
    }

}
