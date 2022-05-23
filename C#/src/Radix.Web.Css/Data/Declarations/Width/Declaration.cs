namespace Radix.Web.Css.Data.Declarations.Width;

public interface Declaration { }

public record Declaration<T> : Declaration<Properties.Values.Width, Properties.Width.Value<T>>, Declaration

{

}
