namespace Radix.Web.Css.Data.Declarations.Height;

public interface Declaration { }

public interface Declaration<T> : Declaration<Properties.Values.Height, Properties.Width.Value<T>>, Declaration

{

}
