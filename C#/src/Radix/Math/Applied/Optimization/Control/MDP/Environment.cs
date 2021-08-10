using System.Collections.Generic;

namespace Radix.Math.Applied.Optimization.Control.MDP
{
    public delegate double Reward<S>(S state);

    // MDP modeled categorically : there is a free monoid A generating the actions in 'a. 
    // Distribution is the kleisli category of the canonical Probability / Distribution (Giry) monad.
    // The functor 'a -> (Distribution<'s * float<reward>>) encodes both the set 's and the transition probabilities (including the reward)
    // The MDP models the environment. Although the reward system can be part of the same robot f.i. or organism, it is also considered external to the agent
    // and part of the environment

    /// <summary>
    /// A call to the dynamics function executes the model of the environment once, where the outcome is a state transition with the probability
    /// that transition actually occurs. Its a specialized version of the more general term "experiment", where transitions are the sample space
    /// and the transition probability the probability measure of the experiment. Episodes are equivalent to trials.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public delegate (Transition<S> transition, Probability.Probability transitionProbability) Dynamics<S, in A>(S state, A action);

    public record Environment<S, A>(IEnumerable<S> States, IEnumerable<A> Actions, Dynamics<S, A> Dynamics, Reward<S> Reward, DiscountFactor γ);

}
