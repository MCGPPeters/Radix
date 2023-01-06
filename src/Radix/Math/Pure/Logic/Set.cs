namespace Radix.Math.Pure.Logic;


public interface Set<T>
    where T : Order<T>
{
    public Func<T, bool> Contains { get; }
}
