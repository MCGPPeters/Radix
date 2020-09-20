using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied
{
    public record Outcome<T>(T Value) : Alias<T>(Value);

    /// <summary>
    /// Since the sample space is a set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record SampleSpace<T>(HashSet<T> Value) : Alias<HashSet<T>>(Value);

    /// <summary>
    /// An event is a subset of the sample space
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record Event<T>(SampleSpace<T> sampleSpace, Func<SampleSpace<T>, HashSet<T>> selector) : Alias<HashSet<T>>(selector(sampleSpace));

    public delegate double Probability<T>(Event<T> @event);


    namespace Samples
    {
        public static class Extensions
        {
            public static Probability<T> FromSampleSpace<T>(this SampleSpace<T> _) => _ => 1d;

            public static Event<T> AsEvent<T>(this SampleSpace<T> samplesSpace) => new Event<T>(samplesSpace, s => s);

            
        }
    }
}
