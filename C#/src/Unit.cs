using System;

namespace Radix
{
    public struct Unit : IComparable<Unit>, IEquatable<Unit>
    {
        public static readonly Unit Instance = new Unit();

        public int CompareTo(Unit other)
        {
            return 0;
        }

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "{}";
        }
    }
}
