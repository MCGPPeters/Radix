using System.Diagnostics.CodeAnalysis;

namespace Radix.Data;

public sealed class Some<T> : Option<T>
{

    internal Some([DisallowNull] T value) => Value = value;

    internal T Value { get; }

    /// <summary>
    ///     Type deconstructor, don't remove even though no references are obvious
    /// </summary>
    /// <param name="value"></param>
    public void Deconstruct(out T value) => value = Value;
}
