namespace Radix;

public class Builder<T>
{
    private readonly T _item;

    public Builder(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        this._item = item;
    }

    public static implicit operator T(Builder<T> b)
    {
        return b._item;
    }

    public T Build() =>
        _item;

    public override bool Equals(object? obj) =>
        obj is Builder<T> other && Equals(_item, other._item);

    protected bool Equals(Builder<T> other) =>
        EqualityComparer<T>.Default.Equals(_item, other._item);

    public override int GetHashCode() =>
        EqualityComparer<T>.Default.GetHashCode(_item);
}
