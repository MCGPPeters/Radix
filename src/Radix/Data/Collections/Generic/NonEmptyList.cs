using System.Collections;
using static Radix.Data.Collections.Generic.List.Extensions;

namespace Radix.Data.Collections.Generic;

public sealed record NonEmptyList<T> : List<T>, IEnumerable
    where T : notnull
{
    public NonEmptyList(T head)
    {
        Head = head;
        Tail = new EmptyList<T>();
    }

    public NonEmptyList(T head, List<T> tail)
    {
        Head = head;
        Tail = tail;
    }

    public T this[int index] => this.ElementAt(index);

    public int Count =>
        System.Linq.Enumerable.Count(this);

    public T Head { get; }
    public List<T> Tail { get; }

    IEnumerator IEnumerable.GetEnumerator() => new NonEmptyListEnumerator<T>(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new NonEmptyListEnumerator<T>(this);
}
