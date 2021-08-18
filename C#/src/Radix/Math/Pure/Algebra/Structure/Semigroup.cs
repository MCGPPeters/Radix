﻿using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Algebra.Structure;

public interface Semigroup<A>
{
    static abstract Func<A, A, A> Combine { get; }
}
