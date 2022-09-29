using Radix.Web.Css.Data.Dimensions;

namespace Radix.Web.Css.Data.Declarations.Width;

public record Length<T, U> : Declaration<Length<U>>
    where U : Literal<U>, Units.Length.Unit<U>
    where T : Literal<Property>
{
    public new Properties.Width.Value<Length<U>>? Value { get; init; }
}

