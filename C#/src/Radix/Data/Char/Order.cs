namespace Radix.Data.Char
{
    public class Order : Order<char>
    {
        public static Func<char, char, Ordering> Compare => (x, y) =>
            Comparer<char>.Default.Compare(x, y) switch
            {
                < 0 => new LT(),
                0 => new EQ(),
                > 0 => new GT()
            };

        public static Func<char, char, bool> Equal => (x, y) => EqualityComparer<char>.Default.Equals(x, y);

        public static Func<char, char, bool> NotEqual => (x, y) => !Equal(x, y);
    }
}
