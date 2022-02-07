namespace Radix.Math.Pure.Logic;

public sealed record Element<T, TSet>
    where T : Order<T>
    where TSet : Set<T, TSet>
{
    private Element(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public static Option<Element<T, TSet>> Get(T candidate, Set<T, TSet> set) =>
        set.Builder(candidate)
        ? Some(new Element<T, TSet>(candidate))
        : None<Element<T, TSet>>();

}
