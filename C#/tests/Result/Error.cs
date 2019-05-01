using System.Linq;

namespace Radix.Tests.Result
{
    public class Error<T> : Monoid<Error<T>>, Result<T>
    {
        internal Error(params string[] messages)
        {
            Messages = messages;
        }

        public string[] Messages { get; }

        public override Error<T> Identity => Messages;

        public static implicit operator Error<T>(string message)
        {
            return new Error<T>(message);
        }

        public static implicit operator Error<T>(string[] messages)
        {
            return messages.Select(m => new Error<T>(m))
                .Aggregate((current, next) => current.Append(next));
        }

        public static implicit operator string[](Error<T> error)
        {
            return error.Messages;
        }

        public override Error<T> Append(Error<T> t)
        {
            return new Error<T>(Messages.Concat(t.Messages).ToArray());
        }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="messages"></param>
        public void Deconstruct(out string[] messages)
        {
            messages = Messages;
        }
    }
}
