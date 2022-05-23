namespace Radix.Web.Css.Data.Declarations.PaddingLeft;

public interface Declaration : Data.Declaration { }

public record Declaration<T> : Declaration<Properties.Values.PaddingLeft, Properties.PaddingLeft.Value<T>>, Declaration
{

}
