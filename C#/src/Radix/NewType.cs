namespace Radix
{

    /// <summary>
    /// Type constructor (type-level application)
    ///
    /// In C# the expression T<U>,
    /// e.g. a generic type parameter with a generic type parameter (where T could be any generic type with 1 generic type parameter like IEnumerable<typeparam name="U" />),
    /// is not allowed.
    /// This type works around this shortcoming by applying type U to T, where T is called the brand in object algebra terminology
    /// Brand<typeparam name="U" /> where a concrete example would be IEnumerable<typeparam name="U" />, where IEnumerable is the Brand
    /// The
    /// </summary>
    /// <typeparam name="Brand"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface Kind<Brand, U> { }


    /// <summary>
    /// Higher-kinded type constructor
    /// </summary>
    /// <typeparam name="T">The generic parameter</typeparam>
    /// <typeparam name="Kind">The kind the generic parameter is applied to</typeparam>
    public class NewType<T, Kind>
    {

    }
}
