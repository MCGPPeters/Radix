using System;

namespace Radix
{
    public struct GarbageCollectionSettings
    {
        public Minutes ScanInterval { get; set; }
        public TimeSpan IdleTimeout { get; set; }
    }
}
