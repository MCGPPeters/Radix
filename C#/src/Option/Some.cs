namespace Radix.Option
{

    public readonly struct Some<T> : Option<T> where T : notnull
    {
        internal T Value { get; }

        internal Some(T value)
        {
            Value = value;
        }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out T value)
        {
            value = Value;
        }
    }

}
