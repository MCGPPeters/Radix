using System;

namespace Radix.Tests.Models
{
    public interface IEvent
    {
        public Guid Id => Guid.NewGuid();
    }
}
