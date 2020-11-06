namespace Radix.Math.Applied.Probability

// A figure of merit is a quantity used to characterize the performance
open Radix.Collections

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

    let cata p f =
        let (Probability p') = p
        f p'

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

module Distribution =

    let return' x = Distribution [(x, Probability 1.0)]

    let bind (prior: Distribution<'a>) (f: 'a -> Distribution<'b>) : Distribution<'b> =

        Distribution [
            let (Distribution prior') =  prior
            for  (Event x, Probability p) in prior' do
            let  (Distribution posterior) = f x
            for (Event y, Probability q) in posterior do
            yield (Event y, Probability (p * q))
        ]

    let (>>=) prior f =
        bind prior f

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

    let toList d =
        let (Distribution eventProbabilities) = d
        eventProbabilities

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

    let shape f = function
        | [] -> impossible
        | xs ->
            let xs'= List.map (fun x -> Event x) xs
            let length = List.length xs
            let incr = 1.0 / float ((length - 1))
            let probabilities = Seq.map f (iterate (fun x -> incr + x) 0.0)
            Distribution.scale (Seq.zip xs' (probabilities |> Seq.map (fun x -> Probability x) |> Seq.take length) |> Seq.toList |> Distribution)

    open Radix.Prelude

    let uniform a = a |> shape (const' 1.0)

    let normalCurve mean stddev x  =
        1.0 / sqrt (2.0 * System.Math.PI) * exp ((-1.0 / 2.0) * System.Math.Pow((x - mean) / stddev, 2.0))

    let normal a = a |> shape (normalCurve 0.5 0.5)

    let filter (Distribution distribution) predicate : Distribution<'a> =
       distribution |> List.filter predicate |> Distribution

module Sampling =

    type Randomized<'a> = Randomized of 'a

    let Guid = Randomized (System.Guid.NewGuid())

    let rec scan (Probability probability ) (Distribution distribution) =
        match distribution with
        | [] -> []
        | (a, Probability probability')::ys ->
                match (probability <= probability') with
                | true ->
                    let (Event e) = a
                    [e]
                | _ -> scan (Probability (probability - probability')) (Distribution ys)


    let select distribution probability =
        scan probability distribution

    let choose distribution =
        let r = System.Random () //DevSkim: ignore DS148264
        select distribution (Probability (r.NextDouble()))
            |> List.map (fun x ->  Randomized x)
            |> List.head


    let random t = t >> choose
