namespace Radix
{
    public class Error
    {

        internal Error(string Message)
        {
            this.Message = Message;
        }

        public string Message { get; }

        public override string ToString()
        {
            return Message;
        }

        public static implicit operator Error(string m)
        {
            return new Error(m);
        }
    }
}
