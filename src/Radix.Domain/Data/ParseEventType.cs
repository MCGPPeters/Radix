using Radix.Data;

namespace Radix.Domain.Data;

public delegate Option<T> Parse<out T, in TFormat>(TFormat input);
