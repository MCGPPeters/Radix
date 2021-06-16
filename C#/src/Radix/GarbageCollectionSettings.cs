using System;

namespace Radix
{
    public class GarbageCollectionSettings
    {

        public TimeSpan ScanInterval { get; set; } = TimeSpan.FromMinutes(1);

        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(2);
    }
}
