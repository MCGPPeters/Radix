using System.Collections.Generic;


namespace Radix.Validated
{ public readonly struct Invalid<T> : Validated<T>
    {
        internal Invalid(IEnumerable<string> reasons)
        {
            Reasons = new List<string>(reasons);
        }

        public static implicit operator Invalid<T>(List<string> reasons)
        {
            return new Invalid<T>(reasons);
        }

        public static implicit operator List<string>(Invalid<T> invalid)
        {
            return invalid.Reasons;
        }

        public List<string> Reasons { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="reasons"></param>
        public void Deconstruct(out List<string> reasons)
        {
            reasons = Reasons;
        }
    }
}
