namespace Radix.Data;

public interface Read<T>
{
    /// <summary>
    /// Parse the string <param name="s"></param>
    /// </summary>
    /// <returns>A validated outcome</returns>
    static abstract Validated<T> Parse(string s);

    /// <summary>
    /// Parse the string <param name="s"></param> and return a validated outcome. The validation message returned when
    /// parsing fails is <param name="validationErrorMessage"></param>
    /// </summary>
    /// <returns></returns>
    static abstract Validated<T> Parse(string s, string validationErrorMessage);
}
