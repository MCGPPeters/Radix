namespace Radix
{
    public delegate T Parse<out T, in TFormat>(TFormat input);
}
