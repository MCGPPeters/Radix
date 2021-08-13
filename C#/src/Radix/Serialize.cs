namespace Radix;

public delegate TFormat Serialize<in T, out TFormat>(T input);
