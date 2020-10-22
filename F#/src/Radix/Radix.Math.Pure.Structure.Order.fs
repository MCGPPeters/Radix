namespace Radix.Math.Pure.Structure.Order
open System

module Interval =

    let inline open' (lower: 'a) (upper: 'a) x =
        if x > lower && x < upper then Some x else None

    let inline rightClosed (lower: 'a) (upper: 'a) x =
        if x > lower && x <= upper then Some x else None

    let inline rightOpen (lower: 'a) x =
        if x > lower then Some x else None

    let inline leftOpen (upper: 'a) x =
        if x < upper then Some x else None

    let inline closed (lower: 'a) (upper: 'a) x =
        if x >= lower && x <= upper then Some x else None
