namespace Radix
{
    public class Error
    {

        internal Error(string message) => this.Message = message;

        public string Message { get; }

        public override string ToString() => Message;

        public static implicit operator Error(string m) => new Error(m);
    }
}
