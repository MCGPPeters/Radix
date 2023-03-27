namespace Radix.Generators;



public static class StringExtensions
{
    public static string FirstCharacterToLowerCase(this string s) =>
        char.ToLowerInvariant(s[0]) + s.Substring(1);
}
