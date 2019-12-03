namespace Radix.Tests.Choice
{
    public static class Extensions<T, U>
    {
        public static Choice<T, U> Either(T t)
        {
            return new Either<T, U>(t);
        }

        public static Choice<T, U> Or(U u)
        {
            return new Or<T, U>(u);
        }
    }
}
