namespace Radix.Web.Css.Data.Declarations.PaddingBottom;

public interface Declaration { }

public record Declaration<T> : Declaration<Properties.Values.PaddingBottom, Properties.PaddingBottom.Value<T>>, Declaration

{

}
