namespace Radix.Math.Applied.Probability

// A figure of merit is a quantity used to characterize the performance
open MassTransit
open Radix.Collections
open Radix.Collections.NonEmpty

type Merit<'a when 'a : comparison> = Merit of 'a

type Probability = float

type Expectation<'e> = Expectation of 'e

type Event<'a> = Event of 'a * Probability
    // with
    //     static member inline Zero = Event(LanguagePrimitives.GenericZero, 0.0)


type Distribution<'a> = Distribution of NonEmpty<Event<'a>>

type Transition<'a> = 'a -> Distribution<'a>

type Spread<'a> = 'a list -> Distribution<'a>

// a sample space which is the set of all possible outcomes. I
type Samples<'a> = Samples of 'a list

type Experiment<'a> = 'a list -> Distribution<'a>

// is a function that maps an event or values of one or more variables 
// onto a real number intuitively representing some benefit gained associated with the event.
type Benefit<'a, 'TMerit when 'TMerit: comparison> = Event<'a> -> Merit<'TMerit>

type Sample< ^a when ^a: comparison > = 
    Sample of Set<'a>

module Event =
    let inline combine(f: 'a -> 'a -> 'b) (Event (x, p)) (Event (y, q)) : Event<'b> =
        Event (f x y, p * q)

    let inline bind(Event (x, p)) (f: 'a -> Event<'b>) : Event<'b> =
        let y = f x
        let (Event( y', q)) = y
        Event (y', q * p)

module Distribution = 

    let inline certainly a = Distribution (Singleton (Event (a, 1.0)))

    let inline uniform list =
        let length = NonEmpty.length list 
        list |> NonEmpty.map (fun e -> Event (e , (1.0 / float length))) |> Distribution                  

    let inline combine f (Distribution d) (Distribution d') : Distribution<'b> =
        Distribution (NonEmpty.combine (Event.combine f) d d')                             

    let pair x y = (x, y) 
    
    // https://queue.acm.org/detail.cfm?id=3055303
    let inline bind (prior: Distribution<'a>) (likelihood: 'a -> Distribution<'b>) : Distribution<'b> =
        
        let x = nonEmpty {
            let (Distribution prior') =  prior
            for Event (x, p) in prior' do
            let  (Distribution posterior) = likelihood x
            for Event (y, q) in posterior do
            return Event (y, q * p)
        }

        Distribution x                  

    ///mapD :: (a -> b) -> Dist a -> Dist b
    let inline map(f: 'a -> 'b) (distribution: Distribution<'a>) : Distribution<'b> = 
    
        let events = nonEmpty {
            let (Distribution distribution') =  distribution
            for Event (x, p) in distribution' do
            for y in Singleton (f x) do 
            return Event (y, p)
        }

        Distribution events                    

    let inline filter (Distribution distribution) predicate : Distribution<'a> =
       distribution |> NonEmpty.filter predicate |> Distribution

    let inline (?) (predicate: Event<'a> -> bool) (Distribution distribution) : Probability =
       let matches = distribution 
                    |> NonEmpty.filter predicate 
                    |> NonEmpty.map (fun (Event(_,p)) -> p)
       matches |> NonEmpty.sumBy id                                

    let inline argMax (Distribution distribution) =
        distribution |> NonEmpty.maxBy (fun (Event(x,_)) -> x)

    type DistributionMonadBuilder() =
        member inline __.Bind (m, f) = bind m f
        member inline __.Return x = certainly x

        member inline __.Map (f, m) = map f m
        member __.ReturnFrom m = m


module Sampling = 

    type Randomized<'a> = Randomized of 'a

    let Guid = Randomized (NewId.Next())

    let rec scan (probability: Probability) (Distribution distribution) =
        match distribution with 
        | Singleton (Event (x, _)) -> x 
        | List (Event (x, probability'), y::ys) -> 
                match (probability <= probability') || y::ys = List.empty with
                | true -> x
                | _ -> scan (probability - probability') (Distribution (List(y, ys)))
        | List (Event (x, _), []) -> x
                    

    let inline select distribution probability = 
        scan probability distribution

    let inline pick distribution = 
        let r = System.Random()
        Randomized(select distribution (r.NextDouble()))

    let inline random t = t >> pick    