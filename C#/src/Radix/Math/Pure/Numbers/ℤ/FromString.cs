using Radix.Data;

namespace Radix.Math.Pure.Numbers.ℤ;

public class FromString : FromString<int>
{
    public static Result<int, Error> Parse(string s) =>
        int.TryParse(s, out var i)
            ? Ok<int, Error>(i)
            : Error<int, Error>($"The string '{s}' can not be parsed. The value is not a valid integer");
}
