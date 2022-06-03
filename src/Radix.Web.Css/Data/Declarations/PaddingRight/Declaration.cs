namespace Radix.Web.Css.Data.Declarations.PaddingRight;

public interface Declaration { }

public record Declaration<T> : Declaration<Properties.Values.PaddingRight, Properties.PaddingRight.Value<T>>, Declaration

{

}
