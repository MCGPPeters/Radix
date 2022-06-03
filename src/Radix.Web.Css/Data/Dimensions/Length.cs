namespace Radix.Web.Css.Data.Dimensions;

public interface Length { };

public interface Length<T> : Dimension<T>, Length
    where T : Literal<T>, Units.Length.Unit<T> { }
