module Radix.Math.Pure.Algebra.Structure
open System.Text.RegularExpressions

type Semigroup<'a> = 
    abstract combine:  'a -> 'a -> 'a

type Monoid<'a> =
    inherit Semigroup<'a>
    abstract zero: 'a

module Monoid = 

    type Monoid<'a> with
        member x.sum a = a |> List.fold x.combine x.zero

type Group<'a> =
    inherit Monoid<'a>
    // inverse under addition
    abstract negate: 'a -> 'a   

module Group = 

    type Group<'a> with 
        member inline x.subtract a b = x.negate a |> x.combine b
        

type Ring<'a> = 
    inherit Group<'a>
    abstract multiply: 'a -> 'a -> 'a
    abstract one : 'a


type Field<'a> = 
    inherit Ring<'a>
    // inverse under multiplication
    abstract invert: 'a -> 'a

module Field = 
    type Field<'a> with 
        member x.divide a b = x.negate a |> x.multiply b