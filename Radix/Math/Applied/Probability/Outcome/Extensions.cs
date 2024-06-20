namespace Radix.Math.Applied.Probability.Event;

public static class Extensions
{
    public static Outcome<U> Map<T, U>(this Outcome<T> e, Func<T, U> project)
        => new(project(e.Value));

    public static Outcome<U> Select<T, U>(this Outcome<T> e, Func<T, U> project)
        => Map(e, project);
}
