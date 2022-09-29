namespace Radix.Web.Css.Data;

/// <summary>
/// A dimension is a number with a unit attached to it. For example, 45deg, 5s, or 10px. dimension is an umbrella category that includes the length, angle, time, and resolution types.
/// </summary>
/// <typeparam name="T">The unit type that goes with this type of dimension</typeparam>
public interface Dimension<out T> : Value
    where T : Unit<T>, Literal<T>
{
    Number Number { get; init; }
};
