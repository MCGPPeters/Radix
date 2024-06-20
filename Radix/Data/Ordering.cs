namespace Radix.Data;

public interface Ordering
{
}

public sealed class LT : Ordering { };
public sealed class EQ : Ordering { };
public sealed class GT : Ordering { };

