using System;

namespace Radix
{
    public struct GarbageCollectionSettings
    {
        public TimeSpan ScanInterval { get; set; }
        public TimeSpan IdleTimeout { get; set; }
    }
}
