namespace Radix.Web.Css.Data.Declarations.PaddingTop;

public interface Declaration { }

public record Declaration<T> : Declaration<Properties.Values.PaddingTop, Properties.PaddingTop.Value<T>>, Declaration

{

}
