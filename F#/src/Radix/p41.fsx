#load "Radix.fs"
#load "Radix.Collections.List.fs"
#load "Radix.Control.Monad.State.fs"
#load "Radix.Math.Applied.Probability.fs"
#load "Radix.Control.Monad.Distribution.fs"
#load "Radix.Math.Applied.Learning.Reinforced.fs"


type Action =
| Start
| Up
| Down
| Left
| Right

open System.Collections
open Radix.Math.Applied.Learning.Reinforced
open Radix.Math.Applied.Probability
open Radix.Math.Applied.Probability.Generators

let gridDynamics : Dynamics<int, Action> =
    Dynamics (fun state -> fun action ->
        match action with
        | Start -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
        | Up ->
            match state with
            | state' when [1..3] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state', -1.0<reward>)))
            | state' when [5..14] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state' - 4, -1.0<reward>)))
            | state' when state' = 4
                -> TransitionProbabilities (certainly (Event (0, -1.0<reward>)))
            | _ -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
        | Down ->
            match state with
            | state' when [12..14] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state', -1.0<reward>)))
            | state' when [1..10] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state' - 4, -1.0<reward>)))
            | state' when state' = 11
                -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
            | _ -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
        | Left ->
            match state with
            | state' when [4; 8; 12] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state', -1.0<reward>)))
            | state' when [2; 3; 5; 6; 7; 9; 10; 11; 13; 14] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state' - 1, -1.0<reward>)))
            | state' when state' = 1
                -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
            | _ -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
        | Right ->
            match state with
            | state' when [4; 8; 12] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state', -1.0<reward>)))
            | state' when [1; 2; 4; 5; 6; 8; 9; 10; 12; 13] |> List.contains state'
                -> TransitionProbabilities (certainly (Event (state' + 1, -1.0<reward>)))
            | state' when state' = 14
                -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
            | _ -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
        )

open Radix.Control.Monad.State.Statefull
            
open Radix.Math.Applied.Probability.Sampling
            
let smallGridWorld  = 
    let randomAction = uniform [Up; Down; Left; Right]
    let equiprobableRandomPolicy = Policy (fun _ -> randomAction)       
    let stateValues = [1..14] |> List.map (fun x -> x, 0.0)
    let environment = (Environment{
        State = 1;
        Dynamics = gridDynamics;
        Transition = (fun (TransitionProbabilities x) -> x |> choose)
    })
    Prediction.evaluate  equiprobableRandomPolicy 0.00001 1.0 stateValues environment
            
            
smallGridWorld
