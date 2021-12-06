using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix;

public record struct Writer<T, TOutput>(T value, TOutput output) where TOutput : Monoid<TOutput>;
