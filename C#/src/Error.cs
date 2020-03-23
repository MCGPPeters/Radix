namespace Radix
{
    public class Error
    {
        public string Message { get; }
        public override string ToString() => Message;

        internal Error(string Message) { this.Message = Message; }

        public static implicit operator Error(string m) => new Error(m);
    }
}
