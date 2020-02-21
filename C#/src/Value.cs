using System;
using System.Runtime.CompilerServices;

namespace Radix
{
    public interface Value<T> : IEquatable<T>, IComparable<T>, IComparable where T : notnull, IComparable, IComparable<T>, IEquatable<T>
    {
        public T Value { get; }

        new int CompareTo(T other) => Value.CompareTo(other);

        new bool Equals(T other)
        {
            var v = ValueTuple.Create(Value);
            var o = ValueTuple.Create(other);
            return v.Equals(o);
        }

        int GetHashCode() => Value.GetHashCode();
        
        bool Equals(object obj) => Value.Equals(obj);
        
        bool IEquatable<T>.Equals(T other) => Equals(other);
        
        int IComparable<T>.CompareTo(T other) => CompareTo(other);

        int IComparable.CompareTo(object other) => Value.CompareTo(((Value<T>)other).Value);

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
