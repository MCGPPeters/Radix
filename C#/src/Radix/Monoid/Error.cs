namespace Radix.Data
{
    public class Error<T, TError> : Result<T, TError>
    {
        internal Error(TError error) => Value = error;

        public TError Value { get; }

        public static implicit operator Error<T, TError>(TError t) => new(t);

        public static implicit operator TError(Error<T, TError> ok) => ok.Value;

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="error"></param>
        public void Deconstruct(out TError error) => error = Value;
    }
}
