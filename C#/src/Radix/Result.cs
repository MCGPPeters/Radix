namespace Radix;

/// The result interface ensures the following
/// - a common base type for the Error and Ok types, so that the type pattern can be used for pattern matching
/// - the generic type parameter can be made covariant (allowing subtypes as a value fot the generic type parameter to match as well)
public interface Result<T, out TError>
{
}
