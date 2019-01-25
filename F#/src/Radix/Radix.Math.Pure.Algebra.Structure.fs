module Radix.Math.Pure.Algebra.Structure

[<RequireQualifiedAccess>] 
module Semigroup = 

    /// Generalization of addition
    let inline combine (a: ^a) (b: ^a)  =
        (^a : (static member combine: ^a -> ^a -> ^a) (a, b))

[<RequireQualifiedAccess>] 
module Monoid = 

    let inline combine (a:^a) (b:^a) = (^a: (static member combine: ^a -> ^a -> ^a) (a, b))

    let inline concat (a:^a list) = (^a: (static member concat: ^a list -> ^a) (a))

    /// Idiomatic F# uses the name Zero for the concept of identity

[<RequireQualifiedAccess>] 
module Group = 

    /// Generalization of addition
    let inline combine (a:^a) (b:^a) = (^a: (static member combine: ^a -> ^a -> ^a) (a, b))

    /// Generalization of negation
    let inline invert (a:^a) (b:^a) = (^a: (static member invert: ^a -> ^a -> ^a) (a, b))

    /// Idiomatic F# uses the name Zero for the concept of identity