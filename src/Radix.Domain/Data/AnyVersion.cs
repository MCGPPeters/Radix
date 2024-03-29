namespace Radix.Domain.Data;

public record AnyVersion() : Version(-2)
{
    public static implicit operator long(AnyVersion anyVersion) => -2;
}
