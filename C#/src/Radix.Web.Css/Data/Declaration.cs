namespace Radix.Web.Css.Data;

public interface Declaration { }

public interface Declaration<T, U> : Declaration
    where T : Property
    where U : Value
{
    public T Property { get; init; }
    public U Value { get; init; }
}
