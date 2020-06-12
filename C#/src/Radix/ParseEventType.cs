namespace Radix
{
    public delegate Option<T> Parse<out T, in TFormat>(TFormat input);
}
