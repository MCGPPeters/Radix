using System;

namespace Radix.Tests
{
    public struct Address
    {
        public bool Equals(Address other)
        {
            return _guid.Equals(other._guid);
        }

        public override bool Equals(object obj)
        {
            return obj is Address other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public static bool operator ==(Address left, Address right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !left.Equals(right);
        }

        private readonly Guid _guid;

        public Address(Guid guid)
        {
            _guid = guid;
        }
    }
}