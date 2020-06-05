namespace Radix.Validated
{
    public class Invalid<T> : Validated<T>
    {
        internal Invalid(params string[] reasons) => Reasons = reasons;

        public static implicit operator Invalid<T>(string[] reasons) => new Invalid<T>(reasons);

        public static implicit operator string[](Invalid<T> invalid) => invalid.Reasons;

        public string[] Reasons { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="reasons"></param>
        public void Deconstruct(out string[] reasons) => reasons = Reasons;
    }
}
