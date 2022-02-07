namespace Radix.Math.Pure.Logic;


public interface Set<T, TSet>
    where T : Order<T>
    where TSet : Set<T, TSet>
{
    public Func<T, bool> Builder { get; }
}
