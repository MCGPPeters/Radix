using System;

namespace Radix.Math.Applied.Probability
{
    public static class EventExtensions
    {
        public static Event<U> Map<T, U>(this Event<T> e, Func<T, U> project)
         => new Event<U>(project(e.Value));

        public static Event<U> Select<T, U>(this Event<T> e, Func<T, U> project)
         => Map(e, project);
    }
}
