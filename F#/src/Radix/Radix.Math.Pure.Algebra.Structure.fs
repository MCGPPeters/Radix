module Radix.Math.Pure.Algebra.Structure

[<RequireQualifiedAccess>] 
module Semigroup =

    type Semigroup<'a> = private Semigroup of 'a

    /// Generalization of addition
    let inline combine (a: ^a) (b: ^a)  =
        (^a : (static member combine: ^a -> ^a -> ^a) (a, b))

    let inline create< ^a
            when ^a: (static member combine: ^a -> ^a -> ^a)> (a: ^a) =
                Semigroup a

[<RequireQualifiedAccess>] 
module Monoid = 
    
    type Monoid<'a> = private Monoid of 'a

    let inline combine (a:^a) (b:^a) = (^a: (static member combine: ^a -> ^a -> ^a) (a, b))

    /// Idiomatic F# uses the name Zero for the concept of identity
    let inline create< ^a
            when ^a: (static member Zero: ^a) 
            and ^a:  (static member combine: ^a -> ^a -> ^a)
            and ^a:  (static member concat: ^a list -> ^a) > (a: ^a) =
                Monoid a

[<RequireQualifiedAccess>] 
module Group = 
    
    type Group<'a> = private Group of 'a

    /// Generalization of addition
    let inline combine (a:^a) (b:^a) = (^a: (static member combine: ^a -> ^a -> ^a) (a, b))

    /// Generalization of negation
    let inline invert (a:^a) (b:^a) = (^a: (static member invert: ^a -> ^a -> ^a) (a, b))

    /// Idiomatic F# uses the name Zero for the concept of identity
    let inline create< ^a
            when ^a: (static member Zero: ^a) 
            and ^a:  (static member combine: ^a -> ^a -> ^a)
            and ^a:  (static member invert: ^a -> ^a -> ^a) > (a: ^a) =
                Group a