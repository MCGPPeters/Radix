namespace Radix.Math.Pure.Structure.Order

module Interval =

    let (|Open|_|) (lower: 'a) (upper: 'a) x =
        if x > lower && x < upper then Some() else None

    let (|Closed|_|) (lower: 'a) (upper: 'a) x =
        if x >= lower && x <= upper then Some() else None