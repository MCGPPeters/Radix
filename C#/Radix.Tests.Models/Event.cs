
using System;

namespace Radix.Tests.Models
{
    public abstract class Event
    {
        protected Event(Address address)
        {
            Address = address;

        }
        public Address Address { get; }
    }
}
