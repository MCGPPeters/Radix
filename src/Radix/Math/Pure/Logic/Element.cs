using static Radix.Control.Option.Extensions;

namespace Radix.Math.Pure.Logic;

public sealed record Element<T>
    where T : Order<T>
{
    private Element(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public static Option<Element<T>> Get(T candidate, Set<T> set) =>
        set.Contains(candidate)
        ? Some(new Element<T>(candidate))
        : None<Element<T>>();

}
