using System.Linq;

namespace Radix.Tests.Result
{
    public class Error<T, TError> : Result<T, TError>
    {
        internal Error(TError error)
        {
            _error = error;
        }

        private TError _error;
        
        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="errors"></param>
        public void Deconstruct(out TError error)
        {
            error = _error;
        }
    }
}
