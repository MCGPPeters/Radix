namespace Radix.Math.Pure.Numbers

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

type Dual = Dual of double * double

module Dual = 

    let ofDouble (d: double) = 
        Dual (d, 0.0)

    let ofInteger (i: int) = 
        Dual (double i, 0.0)

    let epsilonOne = Dual (0.0, 1.0)        

    let inline real (Dual (a, _)) = a

    let inline dual (Dual (_, b)) = b

    let inline (/) (Dual (a, b)) (Dual (c, d)) =
        Dual (a / c, (b * c - d * a) / c * c)

    let inline (*) (Dual (a, b)) (Dual (c, d)) =
        Dual (a * c, (a * d) + (b * c))

    let inline (+) (Dual (a, b)) (Dual (c, d)) =
        Dual (a + c, b + d) 

    let inline (-) (Dual (a, b)) (Dual (c, d)) =
        Dual (a - c, b - d) 

    