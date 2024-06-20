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

        public void Dispose() {; }

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
        }


        public void Reset() => throw new NotImplementedException();
    }
}
