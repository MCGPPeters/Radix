using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied
{
    public record Outcome<T>(T Value) : Alias<T>(Value);

    public record Samples<T>(HashSet<T> Value) : Alias<HashSet<T>>(Value);
    
    public record Event<T>(Samples<T> sampleSpace, Func<Samples<T>, HashSet<T>> selector) : Alias<HashSet<T>>(selector(sampleSpace));

    public delegate double Probability<T>(Event<T> @event);


    namespace Samples
    {
        public static class Extensions
        {
            public static Probability<T> Probability<T>(this Samples<T> _) => _ => 1d;

            public static Event<T> AsEvent<T>(this Samples<T> samplesSpace) => new Event<T>(samplesSpace, s => { var outcomes = s; return outcomes; });
        }
    }
}
