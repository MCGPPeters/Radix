using System;

namespace Radix
{
    public interface Value<T> : IEquatable<T>, IComparable<T>, IComparable where T : IComparable, IComparable<T>, IEquatable<T>
    {
        public T Value { get; }

        int IComparable.CompareTo(object other) => Value.CompareTo(((Value<T>)other).Value);

        int IComparable<T>.CompareTo(T other) => CompareTo(other);

        bool IEquatable<T>.Equals(T other) => Equals(other);

        new int CompareTo(T other) => Value.CompareTo(other);

        new bool Equals(T other)
        {
            ValueTuple<T> v = ValueTuple.Create(Value);
            ValueTuple<T> o = ValueTuple.Create(other);
            return v.Equals(o);
        }

        int GetHashCode() => Value.GetHashCode();

        bool Equals(object obj) => Value.Equals(obj);

        string ToString() => Value.ToString();

        public static bool operator >(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) == 1;

        public static bool operator <(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) == -1;

        public static bool operator >=(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) >= 0;

        // Define the is less than or equal to operator.
        public static bool operator <=(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) <= 0;

        void Deconstruct(out T value) => value = Value;
    }
}
