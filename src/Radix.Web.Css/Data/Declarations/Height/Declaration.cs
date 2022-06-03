namespace Radix.Web.Css.Data.Declarations.Height;

public interface Declaration { }

public record Declaration<T> : Declaration<Properties.Values.Height, Properties.Height.Value<T>>, Declaration

{

}
