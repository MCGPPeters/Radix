namespace Radix.Math.Pure.Structure.Order
open System

// type Equality<'a> = 
//     abstract member equals: 'a -> 'a -> bool
//     static member (==) (a, b) = Equality<'a>.equals a b

// type Orderings< ^a when ^a: equality> =
    

module Interval =

    let inline (|Open|_|) (lower: 'a) (upper: 'a) x =
        if x > lower && x < upper then Some x else None

    let inline (|RightOpen|_|) (lower: 'a) x =
        if x > lower then Some x else None

    let inline (|LeftOpen|_|) (upper: 'a) x =
        if x < upper then Some x else None       

    let inline (|Closed|_|) (lower: 'a) (upper: 'a) x =
        if x >= lower && x <= upper then Some x else None