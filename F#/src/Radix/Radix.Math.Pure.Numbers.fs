namespace Radix.Math.Pure.Numbers

open Radix.Math.Pure.Algebra.Structure

type Enum<'a> =
    abstract predecessor: 'a
    abstract successor: 'a

// all number are rings -> Monoid under multiplication and a group under addition

/// Natural numbers according to he Peano axioms
type Natural =
    | Zero
    | Successor of Natural
    with
    static member inline (*) (x, y) =
            let multiply = { new Multiplication<Natural> with
                member __.op a b =
                    let rec multiply a b =
                        match (a, b) with
                        | (Zero, _) -> Zero
                        | (_, Zero) -> Zero
                        | (Successor a', Successor b') -> Successor (multiply a' b')
                    multiply a b }
            multiply.op x y
    static member inline (+) (x, y) =
            let add = {
                new Addition<Natural> with
                member __.op a b =
                    let rec add a b =
                        match (a, b) with
                        | (Zero, _) -> b
                        | (_, Zero) -> b
                        | (Successor a', Successor b') -> Successor (add a' b')

                    add a b}
            add.op x y
    interface Enum<Natural> with
        member __.predecessor: Natural =
            match __ with
            | Zero -> failwith "There is no predecessor for Zero"
            | Successor s -> s
        member __.successor: Natural = Successor __
    interface Group<Natural, Addition<Natural>> with
        member __.identity = Zero
        member __.combine =
            {
                new Addition<Natural> with
                member __.op a b = a + b }
        member __.invert a =
            match a with
            | Zero -> Zero
            | _ -> failwith "Negative values are no natural numbers"
    interface Monoid<Natural, Multiplication<Natural>> with
        member __.identity = Successor Zero
        member __.combine =
            {
                new Multiplication<Natural> with
                member __.op a b = a * b }

type Integer = Integer of System.Int32
    with
    static member inline (%) (x, y) =
        let (Integer x') = x
        let (Integer y') = y
        let remainder =  x' % y'
        Integer remainder
    static member (*) (Integer x, Integer y) =
        Integer (x * y)
    static member (/) (Integer x, Integer y) =
        Integer (x / y)
    static member (+) (Integer x, Integer y) =
        Integer (x + y)
    static member (-) (Integer x, Integer y) =
        Integer (x - y)
    static member (~-) x: Integer =
        Integer 0 - x
    interface Enum<Integer> with
        member __.predecessor = __ - (Integer 1)
        member __.successor = __ + (Integer 1)
    interface Group<Integer, Addition<Integer>> with
        member __.identity = Integer 0
        member __.combine =
            {
                new Addition<Integer> with
                member __.op a b = a + b}
        member __.invert a = - a
    interface Monoid<Integer, Multiplication<Integer>> with
        member __.identity = Integer 1
        member __.combine =
            {
                new Multiplication<Integer> with
                member __.op a b = a * b }


module Integer =

    let rec gcd (x:Integer) (y: Integer) =
        match y with
        | Integer 0 -> x
        | _ ->
            gcd y (x % y)

    let rec lcm (x:Integer) (y: Integer) =
        let (Integer x') = x
        let (Integer y') = y
        let absX = Integer (abs x')
        let absY = Integer (abs y')

        (absX / (gcd x y)) * absY

    //module Factorisation =

    //    let inline QS


type Rational = private Rational of {| Numerator: Integer; Denominator: Integer |}
    with
    static member create numerator denominator =
        match denominator with
        | Integer 0 -> Error "Division by zero"
        | _ -> Ok (
                Rational {| Numerator = numerator; Denominator = denominator |})

    static member (*) (Rational x, Rational y) =
        Rational {| Numerator = x.Numerator * y.Numerator; Denominator = x.Denominator * y.Denominator|}

    static member (/) (x, Rational y) =
        let switched = Rational {| Numerator = y.Denominator; Denominator = y.Numerator|}
        x * switched

    static member (+) (Rational x, Rational y) =
        match x.Denominator = y.Denominator with
        | false ->
            let lcm = Integer.lcm x.Denominator y.Denominator
            let xNumerator = ((lcm / x.Denominator) * x.Numerator)
            let yNumerator = ((lcm / y.Denominator) * y.Numerator)
            Rational {| Numerator = xNumerator + yNumerator; Denominator = lcm |}
        | true -> Rational {| Numerator = x.Numerator + y.Numerator; Denominator = x.Denominator |}

    static member (-) (Rational x, Rational y) =
        match x.Denominator = y.Denominator with
        | false ->
            let lcm = Integer.lcm x.Denominator y.Denominator
            let xNumerator = ((lcm / x.Denominator) * x.Numerator)
            let yNumerator = ((lcm / y.Denominator) * y.Numerator)
            Rational {| Numerator = xNumerator - yNumerator; Denominator = lcm |}
        | true -> Rational {| Numerator = x.Numerator - y.Numerator; Denominator = x.Denominator |}

    static member (~-) x =
        (Rational {| Numerator = Integer 0; Denominator = Integer 1|}) - x


    interface Group<Rational, Addition<Rational>> with
        member __.identity = Rational {| Numerator = Integer 0; Denominator = Integer 1|}
        member __.combine =
            {
                new Addition<Rational> with
                member __.op a b = a + b}
        member __.invert a =  -a
    interface Monoid<Rational, Multiplication<Rational>> with
        member __.identity = Rational {| Numerator = Integer 1; Denominator = Integer 1|}
        member __.combine =
            {
                new Multiplication<Rational> with
                member __.op a b = a * b }



module Rational =

    let reduce (Rational x) =
        let d = Integer.gcd x.Numerator x.Denominator
        Rational {| Numerator = x.Numerator % d; Denominator = x.Denominator % d |}

//type Irrational = Irrational of

//type Real =
//    | Rational of Rational
//    | Irrational of Irrational

open Radix.Math.Pure.Structure.Order

/// <summary>
///     A rational number in the open interval (0, 1), e.g. 0 &gt; x &lt; 1
/// </summary>
type ProperFraction =
    private ProperFraction of float
        static member Create n =
            match n with
            | n when 0.0 <= n && n <= 1.0 -> Ok (ProperFraction n)
            | _ -> Error "n is not a proper fraction"
        static member (*) (ProperFraction a, ProperFraction b) = ProperFraction.Create(a * b)

type Dual = private Dual of double * double
    with
    static member (+) (Dual (a, b), Dual (c, d)) =
        Dual (a + c, b + d)
    static member (-) (Dual (a, b), Dual (c, d)) =
        Dual (a - c, b - d)
    static member (*) (Dual (a, b), Dual (c, d)) =
        Dual (a * c, (a * d) + (b * c))
    static member (/) (Dual (a, b), Dual (c, d)) =
        Dual (a / c, (b * c - d * a) / c * c)
    static member Create (a, b) =
        Dual (a, b)
    static member Create a =
        Dual (a, 0.0)
    static member Create (a: int) =
        Dual (double a, 0.0)
    static member real (Dual (a, _)) = a
    static member dual (Dual (_, b)) = b
    interface Group<Dual, Addition<Dual>> with
        member __.identity = Dual (0.0, 0.0)
        member __.combine =
            {
                new Addition<Dual> with
                member __.op a b = a + b}
        member __.invert a = a // todo
    interface Monoid<Dual, Multiplication<Dual>> with
        member __.identity = Dual (1.0, 0.0)
        member __.combine =
            {
                new Multiplication<Dual> with
                member __.op a b = a * b }
