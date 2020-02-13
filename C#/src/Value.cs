using System;

namespace Radix
{
    public interface Value<T> : IEquatable<T>, IComparable<T> where T : notnull, IComparable<T>, IEquatable<T>
    {
        T Value { get; }

        new int CompareTo(T other) => Value.CompareTo(other);

        new bool Equals(T other)
        {
            ValueTuple<T> v = ValueTuple.Create(Value);
            ValueTuple<T> o = ValueTuple.Create(other);
            return v.Equals(o);
        }

        int GetHashCode() => Value.GetHashCode();
        bool Equals(object obj) => Value.Equals(obj);

        bool IEquatable<T>.Equals(T other) => Equals(other);
        int IComparable<T>.CompareTo(T other) => CompareTo(other);

        public static bool operator >(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) == 1;

        public static bool operator <(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) == -1;

        public static bool operator >=(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) >= 0;

        // Define the is less than or equal to operator.
        public static bool operator <=(Value<T> operand1, Value<T> operand2) => operand1.CompareTo(operand2.Value) <= 0;

        

        void Deconstruct(out T value) => value = Value;
    }
}