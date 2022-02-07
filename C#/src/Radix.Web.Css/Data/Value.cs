namespace Radix.Web.Css.Data;

public interface Value { }

public interface Value<out T> : Value
{
}
