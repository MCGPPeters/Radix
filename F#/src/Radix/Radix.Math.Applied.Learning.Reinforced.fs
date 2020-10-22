namespace Radix.Math.Applied.Learning.Reinforced

open Radix.Math.Applied.Probability
open Radix.Math.Applied.Probability.Sampling
open Radix.Math.Applied.Probability.Distribution
open Radix.Collections

// generic variable names are chosen using standard conventions. 's for state, 'a for action


type Value<'x> = Value of Expectation<'x>

// State or utility value
type U<'s> = U of Expectation<'s>

// Action value
type Q<'a> = Q of Expectation<'a>

module Select =

    let inline greedy (Q (Expectation (Random q))) actions =
        actions
        |> List.maxBy (fun a -> q a)

    open Generators

    let inline εGreedy (q: Q<'a>) (ε: float) (actions: 'a list) =
        let (Randomized σ) = uniform [0.0..1.0] |> choose
        if (σ > ε)
            then greedy q actions
            else (uniform actions
            |> choose
            |> fun (Randomized x) -> x)

type Action<'a> =
| Idle
| Action of 'a

[<Measure>]
type reward

// MDP modelled categorically : there is a  free monoid A generating the actions in 'a (for instance an agent).
// Distribution is the kleisli category of the canonical Probability / Distribution (Giry) monad.
// The functor 'a -> (Distribution<'s * float<reward>>) (the transition dynamics) encodes both the set 's and the transition probabilities (including the reward)
// The MDP models the environment. Although the reward system can be part of the same robot f.i. or organism it is also considered external to the agent
// and part of the environment

type TransitionProbabilities<'s> = TransitionProbabilities of Distribution<'s * float<reward>>

type Dynamics<'s, 'a> = Dynamics of ('s -> 'a -> TransitionProbabilities<'s>)

type Environment<'s, 'a> = Environment of ('s * Dynamics<'s, 'a> * (TransitionProbabilities<'s> -> Randomized<('s * float<reward>)>))

module Environment =

    let reset initialState = Environment initialState

    let nextS action (Environment environment)  =
        let (state, (Dynamics dynamics), choose) = environment
        let transitionProbabilities = dynamics state action
        let (Randomized(nextState, reward)) = transitionProbabilities |> choose
        reward, Environment (nextState, Dynamics dynamics, choose)

    open Radix.Control.Monad.State.Statefull

    let next action = state {
        let! environment = getState
        let reward, environment = nextS action environment
        do! putState environment
        return reward
    }


// A policy is a mapping from states to the probabilities of selecting each possible action
type Policy<'s, 'a> = Policy of ('s -> Distribution<'a>)

module Prediction =

    open Radix.Control.Monad.Distribution.Instance
    open Radix.Control.Monad.State.Statefull

    // Iterate policy evaluation
    let rec evaluate policy θ discountFactor stateValues =
        let rec iterate Δ stateValues' =
            match Δ with
            | Δ when Δ < θ -> stateValues'
            | _ ->
                match stateValues' with
                | [] -> stateValues'
                | (environment, value)::svs ->
                        let actionProbabilities = distribution {
                            return! policy environment
                        }
                        let newValue =
                            actionProbabilities
                            |> List.map (fun ap ->
                                    let (Event action , p) = ap
                                    let probability = Probability.cata p id
                                    let statefull = state {
                                        let! reward = Environment.next action
                                        return probability * (float reward) + (discountFactor * value)
                                    }
                                    run statefull environment |> fst
                                    )
                            |> List.sum
                        let Δ' = [Δ; abs(value - newValue)] |> List.max
                        iterate Δ' svs

        iterate 0.0 stateValues



// γ = discount rate

module e41 =

    type Action =
    | Start
    | Up
    | Down
    | Left
    | Right


    open Generators

    let gridDynamics : Dynamics<int, Action> =
        Dynamics (fun state -> fun action ->
            match action with
            | Start -> TransitionProbabilities (certainly (Event (state, -1.0<reward>)))
            | Up ->
                match state with
                | state' when [1..3] |> List.contains state'
                    -> TransitionProbabilities impossible
                | state' when [5..14] |> List.contains state'
                    -> TransitionProbabilities (certainly (Event (state' - 4, -1.0<reward>)))
                | state' when state' = 4
                    -> TransitionProbabilities (certainly (Event (0, -1.0<reward>)))
                | _ -> TransitionProbabilities impossible
            | Down ->
                match state with
                | state' when [12..14] |> List.contains state'
                    -> TransitionProbabilities impossible
                | state' when [1..10] |> List.contains state'
                    -> TransitionProbabilities (certainly (Event (state' - 4, -1.0<reward>)))
                | state' when state' = 11
                    -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
                | _ -> TransitionProbabilities impossible
            | Left ->
                match state with
                | state' when [4; 8; 12] |> List.contains state'
                    -> TransitionProbabilities impossible
                | state' when [2; 3; 5; 6; 7; 9; 10; 11; 13; 14] |> List.contains state'
                    -> TransitionProbabilities (certainly (Event (state' - 1, -1.0<reward>)))
                | state' when state' = 1
                    -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
                | _ -> TransitionProbabilities impossible
            | Right ->
                match state with
                | state' when [4; 8; 12] |> List.contains state'
                    -> TransitionProbabilities impossible
                | state' when [1; 2; 4; 5; 6; 8; 9; 10; 12; 13] |> List.contains state'
                    -> TransitionProbabilities (certainly (Event (state' + 1, -1.0<reward>)))
                | state' when state' = 14
                    -> TransitionProbabilities (certainly (Event (0, - 1.0<reward>)))
                | _ -> TransitionProbabilities impossible)

    open Radix.Control.Monad.State.Statefull

    let reset = state {
        return Environment.reset (1, gridDynamics, fun (TransitionProbabilities x) -> x |> choose)
    }

    let agent s =
        let (Policy uniformPolicy) = Policy (fun _ -> uniform [Up; Down; Left; Right])
        let (Randomized action) = (uniformPolicy s) |> choose
        action

    let rolout = state {



        let! environment = reset

    }
    // let stateValue π s =

        // |> U

    // let evaluatePolicy π δ states =
    //     match states with
    //     | [] ->


// type Gamma = float
// type Epsilon = float
// type Alpha = float

// type Reward = Reward of float

// type Transition<'a> = 'a -> Distribution<'a>

// type Action<'a> =
// | Action of 'a
// | Idle

// type Feature<'s> = Feature of 's

// type State<'s> = State of 's

// type Return<'t> = Return of 't * float

// type Value<'a> = Value of Expectation<Return<'a>>

// // The state value function or utility function
// type U<'s> = U of (State<'s> -> Value<State<'s>>)

// type Q<'s, 'a> = Q of (State<'s> -> Value<State<'s> * Action<'a>> list)

//

// type Decide<'a> = Decide of (Distribution<Action<'a>> -> Action<'a>)

// module Policy =

//     let ofQ (q: Q<'s, 'a>) (normalize: Value<State<'s> * Action<'a>> list -> Distribution<Action<'a>>)  : Policy<'s, 'a> =
//         let (Q qFunction) = q
//         Policy(fun state -> state |> (qFunction >> normalize))

// type Episode<'s, 'a> = Experiment<Policy<'s, 'a>>

// type Experience< ^a> = Experience of Transition<'a> list

// open Root

// type Terminal = bool


// // an effect is caused by the agent when taking an action
// type ActorCommand<'s, 'a> =
// | Observe of Environment: Address<EnvironmentCommand<'s, 'a>> * Transition<'s, 'a>
// and
//     EnvironmentCommand<'s, 'a> =
//     | Effect of Address<ActorCommand<'s, 'a>> * Action<'a>
// and
//     Transition<'s, 'a> = {
//         Origin : State<'s>
//         Destination : State<'s>
//         Reward: Reward
//     }

// type ActorEvent<'s, 'a> =
// | Experienced of Transition<'s, 'a>

// type EnvironmentEvent<'s, 'a> =
// | Transitioned of Transition<'s, 'a>
// | Terminated


// open Operators

// type Environment< ^s, 'a when ^s: (static member Zero: ^s) and ^s : equality> = {
//         Dynamics : Action<'a> -> (Randomized<State<'s>> * Reward) option
//         State: State<'s>
//     }
//     with
//         static member inline Zero =
//             let s: State<'s> = State LanguagePrimitives.GenericZero
//             {
//                 Dynamics = fun _ -> None
//                 State = s
//             }
//         static member inline decide (address, currentVersion, environment, command) =
//             match command with
//                 | Effect (agent, action) ->
//                     match(environment.Dynamics action) with
//                         | Some (Randomized destination, reward) ->
//                             let transition = {
//                                 Origin = environment.State
//                                 Destination = destination
//                                 Reward = reward
//                                 }
//                             agent <-- (Observe (address, transition), currentVersion)
//                             [Transitioned transition]
//                         | None -> [Terminated]
//         static member inline apply (_, __) = ()


// //type Actor<'s, 'a> = {
// //        Actions:  Action<'a> NonEmpty
// //        Policy: Policy<'s, 'a>
// //        Decide: Decide<'a>
// //    }
// //    with
// //        static member inline Zero = {
// //            Policy = Policy (fun _ -> certainly Idle)
// //            Actions = Singleton Idle
// //            Decide = Decide (fun _ -> Idle)
// //        }
// //        static member inline decide (address, state, command) =
// //            match command with
// //                | Observe (environment, transition) ->
// //                    let (Policy policy) = state.Policy
// //                    let (Decide decide) = state.Decide
// //                    let action = transition.Destination |> (policy >> decide) // action occurs after transition, so based on destination state
// //                    environment <-- (Effect (this, action), Version.Any) // no concurrency control, since the environment state will not wait for the agent to react
// //                    [Experienced transition]

// //        // static member inline apply (this, event) =
// //        //     match event with
// //        //     | StateSet state' -> state'

// //type Critic<'s, 'a> = {
// //        Actions:  Action<'a> NonEmpty
// //        Policy: Policy<'s, 'a>
// //        Decide: Decide<'a>
// //    }
// //    with
// //        static member inline Zero = {
// //            Policy = Policy (fun _ -> certainly Idle)
// //            Actions = Singleton Idle
// //            Decide = Decide (fun _ -> Idle)
// //        }
// //        static member inline decide (this, command) =
// //            match command with
// //                | Observe (environment, transition) ->
// //                    let (Policy policy) = this.Policy
// //                    let (Decide decide) = this.Decide
// //                    let action = transition.Destination |> (policy >> decide)
// //                    environment <-- (Effect action, Version.Any) // no concurrency control, since the environment state will not wait for the agent to react
// //                    [Experienced transition]

// //        // static member inline apply (this, event) =
// //        //     match event with
// //        //     | StateSet state' -> state'

// //type Observation<'a> = Observation of 'a





// //module Environment =

// //    let rec episode environment (agent: Address<ActorCommand<'s, 'a>>) observation =
// //        agent <-- observation
// //        let (Randomized next, reward, final) = environment.Dynamics action
// //        match final with
// //        | true ->

// //module TD =
// //    let update (learningRate: Alpha) target (current: Return<'a>) =
// //        let (Return (state, value)) = current
// //        Expectation(Return (state, value + learningRate * (target - value)))

// //    let target (reward: Reward) (discount: Gamma) (next: Return<'a>) =
// //        let (Return(state, value)) = next
// //        let (Reward r) = reward
// //        let expected = r + (discount * value)
// //        Expectation( Return (state, expected))

// //module Agent =

// //    let inline create< 's, 'a > (policy: Policy<'s, 'a>) decide : Actor<'s, 'a> =
// //        let (Policy policy) = policy
// //        policy >> decide

// //module Prediction =

// //    let return' state (rewards: Reward list) (discount: Gamma) =
// //        let r = rewards
// //                |> List.mapi (fun tick (Reward reward) ->
// //                               (pown discount tick) * reward)
// //                |> List.sum
// //        Return (state, r)

// //    let rec find utilities (state: State<'s>) =
// //        match utilities with
// //        | [] -> (state, 0.0)
// //        | (s, value)::_ when s = state -> (state, value)
// //        | _::xs -> find xs state

// //    module TD0 =

// //        let rec step environment (agent: Actor<'s, 'a>) state utilities learningRate discount =

// //            let action = agent state
// //            let (Randomized(next, reward, final)) = environment.Dynamics action
// //            let (Expectation nextReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = next)
// //            let (Expectation currentReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = state)
// //            let (Expectation(Return(_, target))) = TD.target reward discount nextReturn
// //            let utilities' = utilities
// //                             |> List.map (fun (Expectation (Return (s, value))) -> match s = state with
// //                                                                                   | true -> (TD.update learningRate target currentReturn)
// //                                                                                   | false -> (Expectation (Return(s, value))))
// //            match final with
// //            | true -> utilities'
// //            | false -> step environment agent next utilities' learningRate discount



// //                   x

// //    let createPolicy policyMatrix =
// //        Policy(fun (State position) ->
// //            let map = Map.ofList policyMatrix
// //            map.[position])

// //    module Sarsa =

// //        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

// //            let ((state, _), _) = observation
// //            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd

// //            let (Randomized (next, reward, terminal)) = environment.Dynamics action

// //            let observation' = ((next, action), reward)
// //            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd

// //            let (Expectation(q)) = qValues
// //                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action))
// //            let (Expectation(qt1)) = qValues
// //                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction))

// //            let (Expectation(Return(_, target))) = TD.target reward discount qt1
// //            let newQValue = TD.update learningRate target q
// //            let qValues' = qValues
// //                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with
// //                                                                                    | true -> newQValue
// //                                                                                    | false -> Expectation(Return ((s,a), value)))
// //            let (Expectation(Return((_, bestAction), _))) = qValues'
// //                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state)
// //                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)

// //            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
// //                                                                        | true -> (s, bestAction)
// //                                                                        | false -> (s, a))

// //            match terminal with
// //            | true -> policyMatrix', qValues'
// //            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount

// //    module ActorCritic =
// //        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

// //            let ((state, _), _) = observation
// //            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd

// //            let (Randomized (next, reward, terminal)) = environment.Dynamics action

// //            let observation' = ((next, action), reward)
// //            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd

// //            let (Expectation(q)) = qValues
// //                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action))
// //            let (Expectation(qt1)) = qValues
// //                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction))

// //            let (Expectation(Return(_, target))) = TD.target reward discount qt1
// //            let newQValue = TD.update learningRate target q
// //            let qValues' = qValues
// //                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with
// //                                                                                    | true -> newQValue
// //                                                                                    | false -> Expectation(Return ((s,a), value)))
// //            let (Expectation(Return((_, bestAction), _))) = qValues'
// //                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state)
// //                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)

// //            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
// //                                                                        | true -> (s, bestAction)
// //                                                                        | false -> (s, a))

// //            match terminal with
// //            | true -> policyMatrix', qValues'
// //            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount


// module Objective =

//     [<Measure>]
//     type reward

//     type Reward = Merit<float<reward>>
//     type Reward<'a> = Benefit<Transition<'a>, Reward>
