namespace Radix.Math.Applied.Learning.Reinforced

open Radix.Math.Applied.Probability
open Radix.Math.Applied.Probability.Sampling
open Radix.Math.Applied.Probability.Distribution
open Radix.Collections

type Gamma = float
type Epsilon = float
type Alpha = float

type Reward = Reward of float 

type Action<'a> = Action of 'a

type State<'s> = State of 's

type Return<'t> = Return of 't * float

// The state value function or utility function
type U<'s> = U of (State<'s> -> Expectation<Return<State<'s>>>)

type Utilities<'s> = Utilities of Expectation<Return<State<'s>>> list

module U =

    let ofUtilities (Utilities utilities) : U<'s> = 
        U(fun v ->
            utilities |> List.find (fun (Expectation(Return(s,_))) -> s = v))
            

type Q<'s, 'a> = Q of (State<'s> -> (Expectation<Return<Action<'a>>> list))

type StateActionValues<'s, 'a> = StateActionValues of (State<'s> * (Expectation<Return<Action<'a>>> list))

module Q = 

    let ofStateActionValues (stateActionMatrix: StateActionValues<'s, 'a> list) : Q<'s, 'a> =
        Q(fun state ->
            stateActionMatrix 
                |> List.filter (fun (StateActionValues(s, _)) -> s = state) 
                |> List.collect (fun (StateActionValues(_, values)) -> values))


type Origin<'s> = Origin of State<'s>

type Destination<'s> = Destination of State<'s>

type Transition<'s, 'a> = {
    Origin : Origin<'s>
    Destination : Destination<'s>
    Action : Action<'a>
}

type StateActionMatrix<'s, 'a> = (State<'s> * Action<'a>) list

type Policy<'s, 'a> = Policy of (State<'s> -> Distribution<Action< ^a>>)

module Policy =
    let ofStateActionMatrix (stateActionMatrix : StateActionMatrix<'s, 'a>) : Policy<'s, 'a> = 
        Policy(fun (State state) -> 
            let map = Map.ofList stateActionMatrix
            certainly map.[State state])

    let ofQ (q: Q<'s, 'a>) (normalize: Expectation<Return<Action<'a>>> list -> Distribution<Action<'a>>)  : Policy<'s, 'a> =
        let (Q qFunction) = q
        Policy(fun state -> state |> (qFunction >> normalize))

type Episode<'s, 'a> = Experiment<Policy<'s, 'a>>

type Experience< ^a> = Experience of Transition<'a> list

type Agent<'s, 'a> = State<'s> -> Action<'a>

type Observation<'a> = Observation of 'a

type Environment<'s, 'a> = {
    Dynamics: Action<'a> -> Randomized<(State<'s> * Reward * bool)>
    Discount: Gamma
    Actions: Action<'a> list
    Observations: Observation<'a> list
}

module TD = 
    let update (learningRate: Alpha) target (current: Return<'a>) =
        let (Return (state, value)) = current
        Expectation(Return (state, value + learningRate * (target - value)))

    let target (reward: Reward) (discount: Gamma) (next: Return<'a>) = 
        let (Return(state, value)) = next
        let (Reward r) = reward
        let expected = r + (discount * value)
        Expectation( Return (state, expected))

module Agent =
        
    let inline create< 's, 'a > (policy: Policy<'s, 'a>) decide : Agent<'s, 'a> =
        let (Policy policy) = policy
        policy >> decide

module Prediction =

    let return' state (rewards: Reward list) (discount: Gamma) = 
        let r = rewards
                |> List.mapi (fun tick (Reward reward) -> 
                               (pown discount tick) * reward)
                |> List.sum
        Return (state, r)                   

    let rec find utilities (state: State<'s>) =
        match utilities with
        | [] -> (state, 0.0)
        | (s, value)::_ when s = state -> (state, value)
        | _::xs -> find xs state

    module TD0 =  

        let rec step environment (agent: Agent<'s, 'a>) state utilities learningRate discount =

            let action = agent state   
            let (Randomized(next, reward, final)) = environment.Dynamics action
            let (Expectation nextReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = next)
            let (Expectation currentReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = state)
            let (Expectation(Return(_, target))) = TD.target reward discount nextReturn
            let utilities' = utilities 
                             |> List.map (fun (Expectation (Return (s, value))) -> match s = state with 
                                                                                   | true -> (TD.update learningRate target currentReturn)  
                                                                                   | false -> (Expectation (Return(s, value))))
            match final with
            | true -> utilities'
            | false -> step environment agent next utilities' learningRate discount     

    
module Control =

    let inline greedy distribution = 
        let (Event (x, _)) = (distribution |> argMax)
        x

    let inline eGreedy epsilon distribution = 
        let uniform = uniform (List(0.0,[0.1..1.0]))
        let (Randomized sigma) = uniform |> pick
        match sigma > epsilon with
        | true -> let (Event (x, _)) = (distribution |> argMax)
                  x
        | false -> let (Randomized x) = distribution |> pick
                   x    

    let createPolicy policyMatrix = 
        Policy(fun (State position) -> 
            let map = Map.ofList policyMatrix
            map.[position])    

    module Sarsa =

        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

            let ((state, _), _) = observation
            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd      

            let (Randomized (next, reward, terminal)) = environment.Dynamics action

            let observation' = ((next, action), reward)
            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd 

            let (Expectation(q)) = qValues 
                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action)) 
            let (Expectation(qt1)) = qValues 
                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction)) 

            let (Expectation(Return(_, target))) = TD.target reward discount qt1
            let newQValue = TD.update learningRate target q                                  
            let qValues' = qValues 
                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with 
                                                                                    | true -> newQValue  
                                                                                    | false -> Expectation(Return ((s,a), value)))
            let (Expectation(Return((_, bestAction), _))) = qValues' 
                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state) 
                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)  

            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
                                                                        | true -> (s, bestAction)
                                                                        | false -> (s, a))                                                                            
            
            match terminal with
            | true -> policyMatrix', qValues'
            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount 

    module ActorCritic =
        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

            let ((state, _), _) = observation
            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd      

            let (Randomized (next, reward, terminal)) = environment.Dynamics action

            let observation' = ((next, action), reward)
            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd 

            let (Expectation(q)) = qValues 
                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action)) 
            let (Expectation(qt1)) = qValues 
                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction)) 

            let (Expectation(Return(_, target))) = TD.target reward discount qt1
            let newQValue = TD.update learningRate target q                                  
            let qValues' = qValues 
                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with 
                                                                                    | true -> newQValue  
                                                                                    | false -> Expectation(Return ((s,a), value)))
            let (Expectation(Return((_, bestAction), _))) = qValues' 
                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state) 
                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)  

            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
                                                                        | true -> (s, bestAction)
                                                                        | false -> (s, a))                                                                            
            
            match terminal with
            | true -> policyMatrix', qValues'
            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount   


module Objective = 

    [<Measure>]
    type reward

    type Reward = Merit<float<reward>>
    type Reward<'a> = Benefit<Transition<'a>, Reward>