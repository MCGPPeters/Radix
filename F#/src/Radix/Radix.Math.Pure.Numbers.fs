namespace Radix.Math.Pure.Numbers

open Radix.Math.Pure.Structure.Order

/// <summary>
///     A rational number in the open interval (0, 1), e.g. 0 &gt; x &lt; 1
/// </summary>
type ProperFraction = 
    private ProperFraction of float
        static member Create n = 
            match n with
            | Interval.Open 0.0 1.0 -> Ok (ProperFraction n)
            | _ -> Error "n is not a proper fraction"
        static member (*) (ProperFraction a, ProperFraction b) = ProperFraction.Create(a * b)