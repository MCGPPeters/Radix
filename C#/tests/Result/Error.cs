using System.Linq;

namespace Radix.Tests.Result
{
    public interface IError{}

    public class Error<T> : Result<T>
    {
        internal Error(params IError[] errors)
        {
            Errors = errors;
        }

        public IError[] Errors { get; }

        // public static implicit operator Error<T>(IError message)
        // {
        //     return new Error<T>(message);
        // }

        // public static implicit operator string[](Error<T, TError> error)
        // {
        //     return error.Messages;
        // }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="errors"></param>
        public void Deconstruct(out IError[] errors)
        {
            errors = Errors;
        }
    }
}
