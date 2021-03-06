﻿using System;

namespace Radix.Math.Pure.Algebra.Operations
{
    public interface Binary<T>
    {
        Func<T, T, T> Apply { get; }
    }

}
