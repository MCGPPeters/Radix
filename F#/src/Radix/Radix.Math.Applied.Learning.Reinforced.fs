namespace Radix.Math.Applied.Learning.Reinforced

open Radix.Math.Applied.Probability
open Radix.Math.Applied.Probability.Sampling
open Radix.Math.Applied.Probability.Distribution
open Radix.Collections

type Gamma = float
type Epsilon = float
type Alpha = float

type Reward = Reward of float 

type Action<'a> = 
| Action of 'a
| Idle

type Feature<'s> = Feature of 's

type State<'s> = State of 's

type Return<'t> = Return of 't * float

type Value<'a> = Value of Expectation<Return<'a>>

// The state value function or utility function
type U<'s> = U of (State<'s> -> Value<State<'s>>)           

type Q<'s, 'a> = Q of (State<'s> -> Value<State<'s> * Action<'a>> list)

type Policy<'s, 'a> = Policy of (State<'s> -> Distribution<Action< ^a>>)

type Decide<'a> = Decide of (Distribution<Action<'a>> -> Action<'a>)

module Policy =

    let ofQ (q: Q<'s, 'a>) (normalize: Value<State<'s> * Action<'a>> list -> Distribution<Action<'a>>)  : Policy<'s, 'a> =
        let (Q qFunction) = q
        Policy(fun state -> state |> (qFunction >> normalize))

type Episode<'s, 'a> = Experiment<Policy<'s, 'a>>

type Experience< ^a> = Experience of Transition<'a> list

open Root

type Terminal = bool


// an effect is caused by the agent when taking an action
type ActorCommand<'s, 'a> = 
| Observe of Environment: Address<EnvironmentCommand<'s, 'a>> * Transition<'s, 'a> 
and 
    EnvironmentCommand<'s, 'a> = 
    | Effect of Address<ActorCommand<'s, 'a>> * Action<'a>
and
    Transition<'s, 'a> = {
        Origin : State<'s>
        Destination : State<'s>
        Reward: Reward
    }

type ActorEvent<'s, 'a> = 
| Experienced of Transition<'s, 'a>

type EnvironmentEvent<'s, 'a> = 
| Transitioned of Transition<'s, 'a>
| Terminated


open Operators

type Environment< ^s, 'a when ^s: (static member Zero: ^s) and ^s : equality> = {
        Dynamics : Action<'a> -> (Randomized<State<'s>> * Reward) option
        State: State<'s>
    }
    with
        static member inline Zero = 
            let s: State<'s> = State LanguagePrimitives.GenericZero
            {
                Dynamics = fun _ -> None
                State = s
            }
        static member inline decide (address, currentVersion, environment, command) = 
            match command with       
                | Effect (agent, action) -> 
                    match(environment.Dynamics action) with
                        | Some (Randomized destination, reward) ->
                            let transition = {
                                Origin = environment.State
                                Destination = destination
                                Reward = reward
                                }
                            agent <-- (Observe (address, transition), currentVersion)
                            [Transitioned transition]
                        | None -> [Terminated]
        static member inline apply (_, __) = ()


//type Actor<'s, 'a> = {
//        Actions:  Action<'a> NonEmpty
//        Policy: Policy<'s, 'a>
//        Decide: Decide<'a>
//    }
//    with
//        static member inline Zero = {
//            Policy = Policy (fun _ -> certainly Idle)
//            Actions = Singleton Idle
//            Decide = Decide (fun _ -> Idle)
//        }
//        static member inline decide (address, state, command) = 
//            match command with       
//                | Observe (environment, transition) -> 
//                    let (Policy policy) = state.Policy
//                    let (Decide decide) = state.Decide
//                    let action = transition.Destination |> (policy >> decide) // action occurs after transition, so based on destination state
//                    environment <-- (Effect (this, action), Version.Any) // no concurrency control, since the environment state will not wait for the agent to react
//                    [Experienced transition]
                
//        // static member inline apply (this, event) = 
//        //     match event with
//        //     | StateSet state' -> state'

//type Critic<'s, 'a> = {
//        Actions:  Action<'a> NonEmpty
//        Policy: Policy<'s, 'a>
//        Decide: Decide<'a>
//    }
//    with
//        static member inline Zero = {
//            Policy = Policy (fun _ -> certainly Idle)
//            Actions = Singleton Idle
//            Decide = Decide (fun _ -> Idle)
//        }
//        static member inline decide (this, command) = 
//            match command with       
//                | Observe (environment, transition) -> 
//                    let (Policy policy) = this.Policy
//                    let (Decide decide) = this.Decide
//                    let action = transition.Destination |> (policy >> decide)
//                    environment <-- (Effect action, Version.Any) // no concurrency control, since the environment state will not wait for the agent to react
//                    [Experienced transition]
                
//        // static member inline apply (this, event) = 
//        //     match event with
//        //     | StateSet state' -> state'    

//type Observation<'a> = Observation of 'a





//module Environment = 

//    let rec episode environment (agent: Address<ActorCommand<'s, 'a>>) observation =
//        agent <-- observation
//        let (Randomized next, reward, final) = environment.Dynamics action
//        match final with
//        | true -> 

//module TD = 
//    let update (learningRate: Alpha) target (current: Return<'a>) =
//        let (Return (state, value)) = current
//        Expectation(Return (state, value + learningRate * (target - value)))

//    let target (reward: Reward) (discount: Gamma) (next: Return<'a>) = 
//        let (Return(state, value)) = next
//        let (Reward r) = reward
//        let expected = r + (discount * value)
//        Expectation( Return (state, expected))

//module Agent =
        
//    let inline create< 's, 'a > (policy: Policy<'s, 'a>) decide : Actor<'s, 'a> =
//        let (Policy policy) = policy
//        policy >> decide

//module Prediction =

//    let return' state (rewards: Reward list) (discount: Gamma) = 
//        let r = rewards
//                |> List.mapi (fun tick (Reward reward) -> 
//                               (pown discount tick) * reward)
//                |> List.sum
//        Return (state, r)                   

//    let rec find utilities (state: State<'s>) =
//        match utilities with
//        | [] -> (state, 0.0)
//        | (s, value)::_ when s = state -> (state, value)
//        | _::xs -> find xs state

//    module TD0 =  

//        let rec step environment (agent: Actor<'s, 'a>) state utilities learningRate discount =

//            let action = agent state   
//            let (Randomized(next, reward, final)) = environment.Dynamics action
//            let (Expectation nextReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = next)
//            let (Expectation currentReturn) = utilities |> List.find (fun (Expectation (Return (s, _))) -> s = state)
//            let (Expectation(Return(_, target))) = TD.target reward discount nextReturn
//            let utilities' = utilities 
//                             |> List.map (fun (Expectation (Return (s, value))) -> match s = state with 
//                                                                                   | true -> (TD.update learningRate target currentReturn)  
//                                                                                   | false -> (Expectation (Return(s, value))))
//            match final with
//            | true -> utilities'
//            | false -> step environment agent next utilities' learningRate discount     

    
//module Control =

//    let inline greedy distribution = 
//        let (Event (x, _)) = (distribution |> argMax)
//        x

//    let inline eGreedy epsilon distribution = 
//        let uniform = uniform (List(0.0,[0.1..1.0]))
//        let (Randomized sigma) = uniform |> pick
//        match sigma > epsilon with
//        | true -> let (Event (x, _)) = (distribution |> argMax)
//                  x
//        | false -> let (Randomized x) = distribution |> pick
//                   x    

//    let createPolicy policyMatrix = 
//        Policy(fun (State position) -> 
//            let map = Map.ofList policyMatrix
//            map.[position])    

//    module Sarsa =

//        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

//            let ((state, _), _) = observation
//            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd      

//            let (Randomized (next, reward, terminal)) = environment.Dynamics action

//            let observation' = ((next, action), reward)
//            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd 

//            let (Expectation(q)) = qValues 
//                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action)) 
//            let (Expectation(qt1)) = qValues 
//                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction)) 

//            let (Expectation(Return(_, target))) = TD.target reward discount qt1
//            let newQValue = TD.update learningRate target q                                  
//            let qValues' = qValues 
//                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with 
//                                                                                    | true -> newQValue  
//                                                                                    | false -> Expectation(Return ((s,a), value)))
//            let (Expectation(Return((_, bestAction), _))) = qValues' 
//                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state) 
//                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)  

//            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
//                                                                        | true -> (s, bestAction)
//                                                                        | false -> (s, a))                                                                            
            
//            match terminal with
//            | true -> policyMatrix', qValues'
//            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount 

//    module ActorCritic =
//        let rec step<'s, 'a when 's: equality and 's : comparison and 'a: equality> (environment: Environment<'s, 'a>) (observation: ((State<'s> * Action<'a>) * Reward)) policyMatrix decide qValues learningRate discount =

//            let ((state, _), _) = observation
//            let action = policyMatrix |> List.find (fun (s, _) -> s = state) |> snd      

//            let (Randomized (next, reward, terminal)) = environment.Dynamics action

//            let observation' = ((next, action), reward)
//            let nextAction = policyMatrix |> List.find (fun (s, _) -> s = next) |> snd 

//            let (Expectation(q)) = qValues 
//                                   |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (state, action)) 
//            let (Expectation(qt1)) = qValues 
//                                     |> List.find (fun (Expectation(Return((s, a), _))) -> (s, a) = (next, nextAction)) 

//            let (Expectation(Return(_, target))) = TD.target reward discount qt1
//            let newQValue = TD.update learningRate target q                                  
//            let qValues' = qValues 
//                           |> List.map (fun (Expectation(Return((s, a), value))) -> match (s, a) = (state, action) with 
//                                                                                    | true -> newQValue  
//                                                                                    | false -> Expectation(Return ((s,a), value)))
//            let (Expectation(Return((_, bestAction), _))) = qValues' 
//                                                            |> List.filter (fun (Expectation(Return((s, _), _))) -> s = state) 
//                                                            |> List.maxBy (fun (Expectation(Return ((_,_), value))) -> value)  

//            let policyMatrix' = policyMatrix |> List.map (fun (s, a) -> match state = s with
//                                                                        | true -> (s, bestAction)
//                                                                        | false -> (s, a))                                                                            
            
//            match terminal with
//            | true -> policyMatrix', qValues'
//            | false -> step environment observation' policyMatrix' decide qValues' learningRate discount   


module Objective = 

    [<Measure>]
    type reward

    type Reward = Merit<float<reward>>
    type Reward<'a> = Benefit<Transition<'a>, Reward>