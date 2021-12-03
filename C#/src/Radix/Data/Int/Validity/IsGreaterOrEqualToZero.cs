namespace Radix.Data.Int.Validity
{
    public class IsGreaterOrEqualToZero : Validity<int>
    { 
        public static Validated<int> Validate(int value) =>
            value < 0
                ? Invalid<int>($"The value must be greater or equal to 0 (zero)")
                : Valid(value);
    }
}
