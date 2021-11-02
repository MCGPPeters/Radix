namespace Radix.Data.Int
{
    public class FromString : FromString<int>
    {
        public static Validated<int> Parse(string s) =>
            Parse(s, "$The value {s} is not a valid integer");

        public static Validated<int> Parse(string s, string validationErrorMessage) =>
            int.TryParse(s, out var i)
                ? Valid(i)
                : Invalid<int>(validationErrorMessage);
    }
}
