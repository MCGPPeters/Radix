namespace Radix.Web.Css.Data;

public interface Declaration { }

public abstract record Declaration<T, U> : Declaration
    where T : Property, new()
    where U : Value
{
    public T Property { get; } = new();
    public U? Value { get; init; }

    public override string ToString() => $"{Property}: {Value}";
}
