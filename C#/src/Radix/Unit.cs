using System;

namespace Radix
{
    public readonly struct Unit : IComparable<Unit>, IEquatable<Unit>
    {
        public static readonly Unit Instance = new();

        public int CompareTo(Unit other) => 0;

        public bool Equals(Unit other) => true;

        public override bool Equals(object? obj) => obj is Unit;

        public override int GetHashCode() => 0;

        public override string ToString() => "{}";
    }
}
