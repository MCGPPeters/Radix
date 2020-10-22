namespace Radix.Math.Applied.Learning

// open Radix.Math.Pure.Numbers
// open Radix.Math.Pure.Structure.Order

// /// <summary>
// ///     Step size or learning rate. A rational number in the closed interval [0, 1]
// /// </summary>
// type Alpha = private Alpha of float

// module Alpha =
//     let create n =
//         match n with
//         | Interval.Closed 0.0 1.0 x -> Ok (Alpha x)
//         | _ -> Error "Alpha must be rational number in the closed interval [0, 1]"

// /// <summary>
// ///     Discount factor
// /// </summary>
// type Gamma = Gamma of ProperFraction
