namespace Radix.Domain.Data;

public record MinimumVersion() : Version(long.MinValue);