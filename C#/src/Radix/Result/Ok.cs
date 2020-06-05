using System;
using System.Collections.Generic;

namespace Radix.Result
{

    public class Ok<T, TError> : Result<T, TError>
    {
        internal Ok(T t)
        {
            if (t is object)
            {
                Value = t;
            }
            else
            {
                throw new ArgumentNullException(nameof(t));
            }

        }

        public static implicit operator Ok<T, TError>(T t) => new Ok<T, TError>(t);

        public static implicit operator T(Ok<T, TError> ok) => ok.Value;

        public T Value { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out T value) => value = Value;

        public bool Equals(Ok<T, TError>? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Ok<T, TError>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static bool operator ==(Ok<T, TError>? left, Ok<T, TError>? right) => Equals(left, right);

        public static bool operator !=(Ok<T, TError>? left, Ok<T, TError>? right) => !Equals(left, right);
    }

}
