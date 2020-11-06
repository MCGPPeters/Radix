#load "Radix.fs"
#load "Radix.Collections.List.fs"
#load "Radix.Math.Applied.Probability.fs"


open Radix.Prelude
open Radix.Math.Applied.Probability


Generators.shape (fun _ -> 1.0)
Generators.uniform [1..10]

