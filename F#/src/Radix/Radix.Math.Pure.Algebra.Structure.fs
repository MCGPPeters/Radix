module Radix.Math.Pure.Algebra.Structure
open System.Text.RegularExpressions


type Binary<'a> = 
    abstract op: 'a -> 'a -> 'a

type Addition<'a> = 
    inherit Binary<'a>
    
type Multiplication<'a> = 
    inherit Binary<'a>


type Semigroup<'a, 'b when 'b :> Binary<'a>> = 
    abstract combine:  'b

type Monoid<'a, 'b when 'b :> Binary<'a> > =
    inherit Semigroup<'a, 'b>
    abstract identity: 'a

module Monoid = 

    type Product<'a> = 
        inherit Monoid<'a, Multiplication<'a>>

    type Sum<'a> = 
        inherit Monoid<'a, Addition<'a>>

    type Monoid<'a, 'b when 'b :> Binary<'a>> with
        member x.sum a = a |> List.fold x.combine.op x.identity

type Group<'a, 'b when 'b :> Binary<'a>> =
    abstract combine:  'b
    abstract identity: 'a
    abstract invert: 'a -> 'a   

module Group = 

    type Sum<'a> = 
        inherit Group<'a, Addition<'a>>

    type Group<'a, 'b when 'b :> Binary<'a>> with 
        member inline x.subtract a b = x.invert a |> x.combine.op    

type Ring< ^a> = 
    inherit Monoid.Product<'a>
    inherit Group.Sum<'a>





//module Field = 
//    type Field<'a> with 
//        member x.divide a b = x.negate a |> x.combine b