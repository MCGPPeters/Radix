module Radix.Math.Pure.Algebra.Structure

module Semigroup =

    type Semigroup<'a> = private Semigroup of 'a

    /// Generalization of addition
    let inline combine (a: ^a) (b: ^a)  =
        (^a : (static member combine: ^a -> ^a -> ^a) (a, b))

    let inline create< ^a
            when ^a: (static member combine: ^a -> ^a -> ^a)> (a: ^a) =
                Semigroup a

module Monoid = 
    
    type Monoid<'a> = private Monoid of 'a

    let inline combine (a:^a) (b:^a) = (^a: (static member combine: ^a -> ^a -> ^a) (a, b))

    /// Idiomatic F# uses the name Zero for the concept of identity
    let inline create< ^a
            when ^a: (static member Zero: ^a) 
            and ^a:  (static member combine: ^a -> ^a -> ^a)
            and ^a:  (static member concat: ^a list -> ^a) > (a: ^a) =
                Monoid a

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

module Functor = 

    type Functor<'a> = private Functor of 'a

    let inline map (f: ^a -> ^b) (fa: ^c when ^c: (static member map: (^a -> ^b) -> ^c -> ^d)) = ( ^a : (static member map: (^a -> ^b) -> ^c -> ^d) (f, fa))

    let inline create< ^a, ^b, ^c, ^d
            when ^c: (static member map: (^a -> ^b) -> ^c -> ^d)
            and ^d: (static member map: (^a -> ^b) -> ^c -> ^d)> (a: ^a)  =
                Functor a



[<RequireQualifiedAccess>] 
module Maybe =

    open Functor

    type Maybe< ^a> =
    | None
    | Some of Functor<'a>
    with
        static member inline map f maybeA = 
            match maybeA with
            | None -> None
            | Some x -> Some (f x)



