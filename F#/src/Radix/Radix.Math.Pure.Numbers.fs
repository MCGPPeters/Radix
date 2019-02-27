namespace Radix.Math.Pure.Numbers

open Radix.Math.Pure.Algebra.Structure


        

type Enum<'a> = 
    abstract predecessor: 'a 
    abstract successor: 'a 

type Whole =
    | One
    | Successor of Whole
    with
    interface Enum<Whole> with
        member this.predecessor: Whole = 
            match this with
            | One -> failwith "There is no predecessor for One"
            | Successor s -> s
        member __.successor: Whole = Successor __


/// Natural numbers according to he Peano axioms
type Natural = 
    | Zero
    | Successor of Natural
    with
    interface Enum<Natural> with
        member __.predecessor: Natural = 
            match __ with
            | Zero -> failwith "There is no predecessor for Zero"
            | Successor s -> s
        member __.successor: Natural = Successor __
    interface Monoid<Natural> with
        member __.zero = Zero
        member __.combine a b = 
            let rec add a b = 
                match (a, b) with
                | (Zero, _) -> b
                | (_, Zero) -> b 
                | (Successor a', Successor b') -> Successor (add a' b')
            add a b 
    interface Ring<Natural> with
        member __.one = Successor Zero        
        member __.multiply a b =
            let rec multiply a b = 
                match (a, b) with
                | (Zero, _) -> Zero
                | (_, Zero) -> Zero
                | (Successor a', Successor b') -> Successor (multiply a' b')
            multiply a b   
        member __.negate a = 
            match a with
            | Zero -> Zero
            | _ -> failwith "Negative values are no natural numbers"                

type Integer = Integer of System.Int32 
    with
    interface Monoid<Integer> with
        member __.zero = Integer 0
        member __.combine a b = 
            let (Integer a') = a
            let (Integer b') = b
            Integer (a' + b')
    interface Group<Integer> with
        member __.negate a = 
            let (Integer a') = a                              
            Integer(0 - a')
    interface Ring<Integer> with
        member __.multiply a b =
            let (Integer a') = a
            let (Integer b') = b
            Integer (a' * b')
        member __.one = Integer 1
    static member inline (%) (x, y) = 
        let (Integer x') = x
        let (Integer y') = y
        let remainder =  x' % y'
        Integer remainder

module Integer = 

    let rec gcd (x: Integer) (y: Integer) =
        match y with
        | Integer 0 -> x
        | _ -> 
            gcd y (x % y)

type Rational = private Rational of {| Nominator: Integer; Denominator: Integer |}
    with
    static member create nominator denominator =
        match denominator with
        | Integer 0 -> Error "Division by zero"
        | _ -> Ok (
                Rational {| Nominator = nominator; Denominator = denominator |})

    member __.reduce x y = 
        let d = Integer.gcd x y

        Rational {| Nominator = x % d; Denominator = y % d |}
        

open Radix.Math.Pure.Structure.Order

/// <summary>
///     A rational number in the open interval (0, 1), e.g. 0 &gt; x &lt; 1
/// </summary>
type ProperFraction = 
    private ProperFraction of Rational
        static member Create n = 
            match n with
            | Interval.Open 0.0 1.0 x -> Ok (ProperFraction x)
            | _ -> Error "n is not a proper fraction"
        static member (*) (ProperFraction a, ProperFraction b) = ProperFraction.Create(a * b)

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
        member __.zero = Dual (0.0, 0.0)
        member __.combine a b =
                a + b        
    interface Group<Dual> with
        member __.negate a = 
            let (Dual (a, b)) = a
            Dual (a ** -1.0, -(b * a) ** -2.0)           
    interface Ring<Dual> with
        member __.one = Dual (1.0, 0.0)
        member __.multiply a b = a * b 