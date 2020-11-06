using System.Collections.Generic;

namespace Radix.Math.Applied.Optimization.Control
{
    public record Transition<S>(S Origin, S Destination);

    // MDP modelled categorically : there is a  free monoid A generating the actions in 'a. 
    // Distribution is the kleisli category of the canonical Probability / Distribution (Giry) monad.
    // The functor 'a -> (Distribution<'s * float<reward>>) encodes both the set 's and the transition probabilities (including the reward)
    // The MDP models the environment. Although the reward system can be part of the same robot f.i. or organism it is8i also considered external to the agent
    // and part of the environment
    public delegate (Transition<S>, Reward, Option<Probability.Probability>) Dynamics<S, A>(S state, A action);

    public record MDP<S, A>(IEnumerable<S> States, IEnumerable<A> Actions, Dynamics<S, A> Dynamics, double DiscountFactor);
}
