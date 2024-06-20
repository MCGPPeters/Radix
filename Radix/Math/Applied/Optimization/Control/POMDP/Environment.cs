using Radix.Data;

namespace Radix.Math.Applied.Optimization.Control.POMDP;

/// <summary>
/// A partially observable Markov decision process (POMDP) is a tuple (S, A, O, T, R, γ) where:
/// S is a finite set of states or the state space
/// A is a finite set of actions or the action space
/// O is a finite set of observations or the observation space. 
///     This is the set of possible observations the agent can receive from the environment
/// T is a function T : S × A × S → [0, 1] that defines the transition probabilities
/// R is a function R : S × A → R that defines the reward function
/// γ ∈ [0, 1] is the discount factor
/// </summary>
/// <typeparam name="State">
///     This is the set of all possible states of the environment.
///     A state is the complete description of everything about the environment at a particular time step.
///     In a fully observable environment, the state is the same as the observation.
///     In the context of the Blackjack game (Radix.Tests), the state could include the dealer's cards, the player's cards, 
///     and whether the game is over.
/// </typeparam>
/// <typeparam name="Action">
///     This is the set of all possible actions that an agent can perform.
///     An action is a choice that the agent makes in order to influence the state of the environment.
///     In the Blackjack game (Radix.Tests), the action could be to hit or stand.
/// </typeparam>
/// <typeparam name="Observation">
///     This is the set of all possible observations that an agent can perceive.
///     An observation is the information that the agent receives about the state of the environment. 
///     Depending on the environment, an observation might not include all information about the state. 
///     For example, in a partially observable environment, the agent might not be able to see all of the state's details. 
///     In the Blackjack game (Radix.Tests), the observation could include the dealer's showing card and the player's hand, 
///     but not the dealer's hidden card.</typeparam>
/// <param name="Dynamics">
///     This is the dynamics of the environment.
///     The dynamics of an environment are the rules that govern how the environment changes from one state to another.
///     In the Blackjack game (Radix.Tests), the dynamics could include the rules for how the dealer and player draw cards.
/// </param>
/// <param name="Reward">
///     This is the reward function.
///     The reward function is a function that maps a state and an action to a real number.
///     The reward function is used to evaluate the quality of an action in a particular state,
///     i.e. how good it is for the agent to perform that action in that state.
///     In the Blackjack game (Radix.Tests), the reward function could be +1 if the agent won the game, -1 if the agent lost the game,
///     and 0 if the game is still in progress.
/// </param>
/// <param name="γ">
///     This is the discount factor.
///     The discount factor is a number between 0 and 1 that determines how much the agent values future rewards.
///     A discount factor of 0 means that the agent only cares about the immediate reward.
///     A discount factor of 1 means that the agent cares about all future rewards equally.
///     In the Blackjack game (Radix.Tests), the discount factor could be used to determine how much the agent values
///     winning the game versus winning the next hand.
/// </param>
public record Environment<State, Action, Observation>(MDP.Dynamics<State, Action> Dynamics, Reward<Observation> Reward, DiscountFactor γ);

public delegate Reward Reward<Observation>(Observation observation);

/// <summary>
///     An agent is a function that takes a reward and an observation and returns an action.
///     The agent is the part of the system that decides what action to take based on the reward and observation.
///     In the Blackjack game (Radix.Tests), the agent could be a function that takes the reward and observation and returns
///     an action based on the rules of Blackjack.
/// </summary>
/// <typeparam name="Observation">
///     This is the set of all possible observations that an agent can perceive.
///     An observation is the information that the agent receives about the state of the environment.
///     Depending on the environment, an observation might not include all information about the state.
///     For example, in a partially observable environment, the agent might not be able to see all of the state's details.
///     In the Blackjack game (Radix.Tests), the observation could include the dealer's showing card and the player's hand,
/// </typeparam>
/// <typeparam name="Action">
///     This is the set of all possible actions that an agent can perform.
///     An action is a choice that the agent makes in order to influence the state of the environment.
///     In the Blackjack game (Radix.Tests), the action could be to hit or stand.
/// </typeparam>
/// <param name="reward">
///     This is the reward that the agent receives from the environment.
///     The reward is a real number that represents how good the agent's previous action was.
///     In the Blackjack game (Radix.Tests), the reward could be +1 if the agent won the game, -1 if the agent lost the game,
///     and 0 if the game is still in progress.
/// </param>
/// <param name="observation">
///     This is the observation that the agent receives from the environment.
/// </param>
/// <returns>
///     This is the action that the agent chooses to perform.
///     In the Blackjack game (Radix.Tests), the action could be to hit or stand.
///     The action is a choice that the agent makes in order to influence the state of the environment.
/// </returns>
public delegate Action Agent<in Observation, out Action>(Reward reward, Observation observation);
public delegate Observation Reset<out Observation>();

public delegate Observation Step<State, Observation, Action>(Environment<State, Action, Observation> environment, State state, Action action);

public delegate Unit Render<State, Action, Observation>(Environment<State, Action, Observation> environment);


