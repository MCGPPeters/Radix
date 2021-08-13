namespace Radix.Blazor.Inventory.Server.Pages;

public class IncrementCommand : IComparable, IComparable<IncrementCommand>, IEquatable<IncrementCommand>
{
    public int CompareTo(object? obj) => throw new NotImplementedException();

    public int CompareTo(IncrementCommand? other) => throw new NotImplementedException();

    public bool Equals(IncrementCommand? other) => throw new NotImplementedException();
}
