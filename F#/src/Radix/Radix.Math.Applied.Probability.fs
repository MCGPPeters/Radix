namespace Radix.Math.Applied.Probability

// A figure of merit is a quantity used to characterize the performance
open Radix.Collections
open Radix.Collections.NonEmpty

// The result of a single trial of an experiment
type Outcome<'a> = Outcome of 'a

// The sample space is the set of all possible outcomes of an experiment.
// The entire sample space may be unknown until after the experiment has concluded
type SampleSpace<'a when 'a: comparison> = SampleSpace of 'a Set

// An event is a collection of outcomes and a subset of a sample space
type Event<'a> = Event of 'a

type EventSpace<'a when 'a: comparison> = EventSpace of Event<'a> Set

type Probability = private Probability of float
    with static member Zero = Probability 0.0

module Probability =

    // the probability measure is a value in the interval [0.0..1.0]
    let create (p: float) =
        if p >= 0.0 && p <= 1.0 then Some (Probability p) else None


type Experiment<'a when 'a : comparison> = {
    Samples: SampleSpace<'a>
    Events: EventSpace<'a>
    ProbabilityFunction: Event<'a> -> Probability
}

// A random variable is a real valued function that assigns a numerical value to each possible outcome of an experiment
type Random<'a> = Random of ('a -> float)

// A distribution represents the outcome of a probabilistic
// event as a collection of all possible values, tagged with their likelihood.
type Distribution<'a> = private Distribution of (Event<'a> * Probability) list
    with static member Zero = Distribution []

module Distribution =

    let return' x = Distribution [(x, Probability 1.0)]

    let bind (prior: Distribution<'a>) (likelihood: 'a -> Distribution<'b>) : Distribution<'b> =

        Distribution [
            let (Distribution prior') =  prior
            for  (Event x, Probability p) in prior' do
            let  (Distribution posterior) = likelihood x
            for (Event y, Probability q) in posterior do
            yield (Event y, Probability (p * q))
        ]

    let (>>=) prior likelyhood =
        bind prior likelyhood

    let map(f: 'a -> 'b) (distribution: Distribution<'a>) : Distribution<'b> =

        Distribution [
            let (Distribution distribution') =  distribution
            for (Event x, Probability p) in distribution' do
            for y in f x do
            yield (Event y, Probability p)
        ]

    let sum (Distribution d)  =
        d
        |> List.map (fun (_, Probability p) -> p)
        |> List.sum
        |> Probability

    let scale d =
        let (Probability q) = sum d
        let (Distribution d') = d
        d'
        |> List.map (fun (e, Probability p) -> (e, Probability (p / q)))
        |> Distribution

type Expectation<'x> = Expectation of Random<'x>

    module Discrete =

        let rec expectation (Random f) (Distribution d) : Expectation<'x> =
            match d with
            | [] -> Expectation (Random (fun _ -> 0.0))
            | (Event a, (Probability p))::xs' ->
                let (Expectation (Random g)) = expectation (Random f)(Distribution xs')
                let e = (f a)*p
                let e' = g a
                Expectation (Random (fun _ -> (e + e')))

        let rec variance (Random f) xs =
            match xs with
            | [] -> 0.0
            | x::xs' ->
                f x + variance (Random f) xs'

        let stddv f xs = System.Math.Sqrt <| variance f xs



module Generators =

    type Spread<'a> = 'a list -> Distribution<'a>

    let certainly a = Distribution.return' a

    let impossible = Distribution []

    open Radix.Collections.List

    let rec shape f = function
        | [] -> impossible
        | xs ->
            let xs'= List.map (fun x -> Event x) xs
            let incr = 1.0 / float ((List.length xs - 1))
            let probabilities = List.map f (iterate (fun x -> incr + x) 0.0)
            Distribution.scale (List.zip xs' (probabilities |> List.map (fun x -> Probability x)) |> Distribution)

    open Radix.Prelude

    let uniform () = shape (const' 1.0)

    let normalCurve mean stddev x  =
        1.0 / sqrt (2.0 * System.Math.PI) * exp ((-1.0 / 2.0) * System.Math.Pow((x - mean) / stddev, 2.0))

    let normal () = shape (normalCurve 0.5 0.5)

    let filter (Distribution distribution) predicate : Distribution<'a> =
       distribution |> List.filter predicate |> Distribution


    type DistributionMonadBuilder() =
        member inline __.Bind (m, f) = bind m f
        member inline __.Return x = certainly x
        member inline __.Map (f, m) = map f m
        member __.ReturnFrom m = m


module Sampling =

    type Randomized<'a> = Randomized of 'a

    let Guid = Randomized (MassTransit.NewId.Next())

    let rec scan (Probability probability ) (Distribution distribution) =
        match distribution with
        | [] -> None
        | (a, Probability probability')::ys ->
                match (probability <= probability') with
                | true -> Some a
                | _ -> scan (Probability (probability - probability')) (Distribution ys)


    let select distribution probability =
        scan probability distribution

    let pick distribution =
        let r = System.Random () //DevSkim: ignore DS148264
        Randomized(select distribution (Probability (r.NextDouble())))

    let random t = t >> pick
