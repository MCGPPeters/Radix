namespace Radix.Math.Pure.Numbers

open Radix.Math.Pure.Algebra.Structure

type Enum = 
    abstract predecessor: 'a -> 'a
    abstract successor: 'a -> 'a

type Natural = 
    | Zero
    | Successor of Natural
    with
    interface Enum
        member x.successor = Successor x
        member x.predecessor = 
            match x with
            | Zero -> failwith "There is no predecessor for Zero"
            | Successor s -> s
    interface Ring<Natural> with
        member __.One = Successor Zero
        member __.Zero = Zero
        member a.combine b = 
            let rec add a b = 
                match (a, b) with
                | (Zero, _) -> b
                | (_, Zero) -> b 
                | (Successor a', Successor b') -> Successor (add a' b')
            add a b            
        member a.multiply b =
            let rec multiply a b = 
                match (a, b) with
                | (Zero, _) -> Zero
                | (_, Zero) -> Zero
                | (Successor a', Successor b') -> Successor (multiply a' b')
            multiply a b   
        member a.negate = 
            match a with
            | Zero -> Zero
            | _ -> failwith "Negative values are no natural numbers"                

    // let inline show n = 






open Radix.Math.Pure.Structure.Order

/// <summary>
///     A rational number in the open interval (0, 1), e.g. 0 &gt; x &lt; 1
/// </summary>
type ProperFraction = 
    private ProperFraction of float
        static member Create n = 
            match n with
            | Interval.Open 0.0 1.0 x -> Ok (ProperFraction x)
            | _ -> Error "n is not a proper fraction"
        static member (*) (ProperFraction a, ProperFraction b) = ProperFraction.Create(a * b)



type Int = Int of System.Int32 
    with
    interface Monoid<Int> with
        member __.Zero = Int 0
        member __.combine a b = 
            let (Int a') = a
            let (Int b') = b
            Int (a' + b')
    interface Group<Int> with
        member __.negate a = 
            let (Int a') = a                              
            Int(-a')
    interface Ring<Int> with
        member __.multiply a b =
            let (Int a') = a
            let (Int b') = b
            Int (a' * b')
        member __.One = Int 1

type Dual = private Dual of double * double
    with
    static member inline (+) (Dual (a, b), Dual (c, d)) =
        Dual (a + c, b + d)
    static member inline (-) (Dual (a, b), Dual (c, d)) =
        Dual (a - c, b - d)     
    static member inline (*) (Dual (a, b), Dual (c, d)) =
        Dual (a * c, (a * d) + (b * c))   
    static member inline (/) (Dual (a, b), Dual (c, d)) =
        Dual (a / c, (b * c - d * a) / c * c)     
    static member Create (a, b) = 
        Dual (a, b) 
    static member Create a = 
        Dual (a, 0.0)
    static member Create (a: int) = 
        Dual (double a, 0.0)   
    static member inline real (Dual (a, _)) = a
    static member inline dual (Dual (_, b)) = b    
    interface Monoid<Dual> with
        member __.Zero = Dual (0.0, 0.0)
        member a.combine b =
                a + b        
    interface Group<Dual> with
        member __.negate (Dual (a, b)) = 
            Dual (a ** -1.0, -(b * a) ** -2.0)           
    interface Ring<Dual> with
        member __.One = Dual (1.0, 0.0)
        member __.multiply a b = a * b 