using System.Collections;
using static Radix.Data.Collections.Generic.List.Extensions;

namespace Radix.Data.Collections.Generic;

public sealed record EmptyList<T> : List<T>
    where T : notnull
{
    public T this[int index] => this.ElementAt(index); 

    public int Count => 0;

    IEnumerator IEnumerable.GetEnumerator() { yield break; }
    IEnumerator<T> IEnumerable<T>.GetEnumerator() { yield break; }
}

