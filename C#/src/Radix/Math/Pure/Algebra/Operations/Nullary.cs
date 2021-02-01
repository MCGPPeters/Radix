namespace Radix.Math.Pure.Algebra.Operations
{
    public delegate T Nullary<out T>();

    public static class Nullary
    {
        public static Nullary<T> Constant<T>(T value) => () => value;
    }
}
