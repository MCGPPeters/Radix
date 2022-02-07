namespace Radix.Web.Css.Data.Declarations.Width;

public interface Declaration { }

public interface Declaration<T> : Declaration<Properties.Values.Width, Properties.Width.Value<T>>, Declaration

{

}
