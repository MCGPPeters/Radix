namespace Radix.Data;

public interface Read<out T>
{
    /// <summary>
    /// Parse the string <param name="s"></param>
    /// </summary>
    /// <param name="s"></param>
    /// <returns>A validated outcome</returns>
    static abstract Validated<T> Parse(string s);

    /// <summary>
    /// Parse the string <param name="s"></param> and return a validated outcome. The validation message returned when
    /// parsing fails is <param name="validationErrorMessage"></param>
    /// </summary>
    /// <param name="s"></param>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    static abstract Validated<T> Parse(string s, string validationErrorMessage);
}
