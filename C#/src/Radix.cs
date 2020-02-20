using System;

namespace Radix
{
    public static class _
    {
        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
        {
            return t1 => t2 => func(t1, t2);
        }

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
        {
            return t1 => t2 => t3 => func(t1, t2, t3);
        }

        public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>
            (this Func<T1, T2, T3, R> @this)
        {
            return t1 => (t2, t3) => @this(t1, t2, t3);
        }

        public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>
            (this Func<T1, T2, T3, T4, R> @this)
        {
            return t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);
        }

        public static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>
            (this Func<T1, T2, T3, T4, T5, R> @this)
        {
            return t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, R>> CurryFirst<T1, T2, T3, T4, T5, T6, R>
            (this Func<T1, T2, T3, T4, T5, T6, R> @this)
        {
            return t1 => (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, R> @this)
        {
            return t1 => (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> @this)
        {
            return t1 => (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> CurryFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> @this)
        {
            return t1 => (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }
    }

    public static class _<T>
    {
        public static Func<T, T> Id => t => t;
    }
}
