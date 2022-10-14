using System.Collections;

namespace Radix.Data.Collections.Generic
{
    public class NonEmptyListEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
        where T : notnull
    {
        private NonEmptyList<T> _list;
        private bool _started = false;

        public NonEmptyListEnumerator(NonEmptyList<T> list)
        {
            _list = list;
        }
        public T Current =>
            GetCurrent();

        private T GetCurrent() =>
            _started
            ? _list.Head
            : throw new InvalidOperationException("Enumeration not yet started");

        object IEnumerator.Current =>
            GetCurrent();

        public void Dispose() {;}

        public bool MoveNext()
        {
            if (_started)
            {
                switch (_list.Tail)
                {
                    case EmptyList<T>:
                        return false;
                    case NonEmptyList<T> ts:
                        _list = ts;
                        return true;
                    default:
                        return false;
                
                }
            }
            else
            {
                _started = true;
                return _list is NonEmptyList<T> && _list is not null;
            }
            return false;
        }
    

        public void Reset() => throw new NotImplementedException();
    }

    //public class NonEmpty<T> : IReadOnlyList<T>, IEnumerable, 
    //{
    //    public T this[int index] => throw new NotImplementedException();

    //    public int Count => throw new NotImplementedException();

    //    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
    //    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    //}

    //public struct NonEmptyEnumerator<T> : IEnumerator<T>
    //{
    //    public T Current => throw new NotImplementedException();

    //    object IEnumerator.Current => throw new NotImplementedException();

    //    public void Dispose() => throw new NotImplementedException();
    //    public bool MoveNext() => throw new NotImplementedException();
    //    public void Reset() => throw new NotImplementedException();
    //}
}

//type List<'T> = 
//       | ([])  :                  'T list
//       | (::)  : Head: 'T * Tail: 'T list -> 'T list
//       interface System.Collections.Generic.IEnumerable < 'T>
//       interface System.Collections.IEnumerable
//       interface System.Collections.Generic.IReadOnlyCollection < 'T>
//       interface System.Collections.Generic.IReadOnlyList < 'T>
