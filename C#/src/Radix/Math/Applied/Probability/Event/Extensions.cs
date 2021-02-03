using System;

namespace Radix.Math.Applied.Probability.Event
{
    public static class Extensions
    {
        public static Event<U> Map<T, U>(this Event<T> e, Func<T, U> project)
            => new(project(e.Value));

        public static Event<U> Select<T, U>(this Event<T> e, Func<T, U> project)
            => Map(e, project);
    }
}
