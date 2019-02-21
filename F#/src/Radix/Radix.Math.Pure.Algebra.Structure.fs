module Radix.Math.Pure.Algebra.Structure

type Semigroup<'a> = 
    abstract  combine:  'a -> 'a

type Monoid<'a> =
    inherit Semigroup<'a>
    abstract Zero: 'a

// module Monoid = 

//     type Monoid with
//         member x.sum a = a |> List.fold x.combine x.Zero

type Group<'a> =
    inherit Monoid<'a>
    // inverse under addition
    abstract negate: 'a

// module Group = 

//     type Group with 
//         member x.subtract a = x.negate a |> x.combine

type Ring<'a> = 
    inherit Group<'a>
    abstract multiply: 'a -> 'a
    abstract One : 'a


type Field<'a> = 
    inherit Ring<'a>
    // inverse under multiplication
    abstract invert: 'a -> 'a

module Field = 
    type Field<'a> with 
        member x.divide a = x.negate a |> x.multiply