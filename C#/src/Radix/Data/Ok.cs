namespace Radix.Data;

public sealed class Ok<T, TError> : Result<T, TError> where T : notnull
{
    internal Ok(T t)
    {
        if (t is not null)
        {
            Value = t;
        }
        else
        {
            throw new ArgumentNullException(nameof(t));
        }

    }

    public T Value { get; }

    public static implicit operator Ok<T, TError>(T t) => new(t);

    public static implicit operator T(Ok<T, TError> ok) => ok.Value;

    /// <summary>
    ///     Type deconstructor, don't remove even though no references are obvious
    /// </summary>
    /// <param name="value"></param>
    public void Deconstruct(out T value) => value = Value;

    public bool Equals(Ok<T, TError>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Ok<T, TError>)obj);
    }

    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

    public static bool operator ==(Ok<T, TError>? left, Ok<T, TError>? right) => Equals(left, right);

    public static bool operator !=(Ok<T, TError>? left, Ok<T, TError>? right) => !Equals(left, right);
}
