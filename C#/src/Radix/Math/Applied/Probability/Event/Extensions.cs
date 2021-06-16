using System;

namespace Radix.Math.Applied.Probability.Event
{
    public static class Extensions
    {
        public static Event<U> Map<T, U>(this Event<T> e, Func<T, U> project) where U : notnull where T : notnull
            => new(project(e.Value));

        public static Event<U> Select<T, U>(this Event<T> e, Func<T, U> project) where U : notnull where T : notnull
            => Map(e, project);
    }
}
