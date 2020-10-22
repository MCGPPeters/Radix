namespace Radix.Control.Monad.Distribution

open Radix.Math.Applied.Probability
open Radix.Math.Applied.Probability.Generators

type DistributionBuilder() =
    member inline __.Zero() = impossible
    member inline __.Bind (m, f) = Distribution.bind m f
    member inline __.Return x = certainly x
    member inline __.Map (f, m) = Distribution.map f m
    member __.ReturnFrom m = m

module Instance =

    let distribution = new DistributionBuilder()
