namespace Radix.Monoid
{
    public class Error<T, TError> : Result<T, TError> where TError : Monoid<TError>
    {
        internal Error(TError error)
        {
            Value = error;
        }

        public static implicit operator Error<T, TError>(TError t)
        {
            return new Error<T, TError>(t);
        }

        public static implicit operator TError(Error<T, TError> ok)
        {
            return ok.Value;
        }

        public TError Value { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="error"></param>
        public void Deconstruct(out TError error)
        {
            error = Value;
        }
    }
}
