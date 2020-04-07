using System;

namespace Radix
{
    public interface Value<T> : IEquatable<T>, IComparable<T>, IComparable where T : notnull, IComparable, IComparable<T>, IEquatable<T>
    {
        public T Value { get; }

        int IComparable.CompareTo(object other)
        {
            return Value.CompareTo(((Value<T>) other).Value);
        }

        int IComparable<T>.CompareTo(T other)
        {
            return CompareTo(other);
        }

        bool IEquatable<T>.Equals(T other)
        {
            return Equals(other);
        }

        new int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        new bool Equals(T other)
        {
            var v = ValueTuple.Create(Value);
            var o = ValueTuple.Create(other);
            return v.Equals(o);
        }

        int GetHashCode()
        {
            return Value.GetHashCode();
        }

        bool Equals(object obj)
        {
            return Value.Equals(obj);
        }

        string ToString()
        {
            return Value.ToString();
        }

        public static bool operator >(Value<T> operand1, Value<T> operand2)
        {
            return operand1.CompareTo(operand2.Value) == 1;
        }

        public static bool operator <(Value<T> operand1, Value<T> operand2)
        {
            return operand1.CompareTo(operand2.Value) == -1;
        }

        public static bool operator >=(Value<T> operand1, Value<T> operand2)
        {
            return operand1.CompareTo(operand2.Value) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Value<T> operand1, Value<T> operand2)
        {
            return operand1.CompareTo(operand2.Value) <= 0;
        }

        void Deconstruct(out T value)
        {
            value = Value;
        }
    }
}
