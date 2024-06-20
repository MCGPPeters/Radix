namespace Radix;

public interface Literal<out T> : IFormattable
{
    public static abstract string Format();

}

