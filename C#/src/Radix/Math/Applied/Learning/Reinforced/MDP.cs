using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Learning.Reinforced
{


    // MDP modelled categorically : there is a  free monoid A generating the actions in 'a. 
    // Distribution is the kleisli category of the canonical Probability / Distribution (Giry) monad.
    // The functor 'a -> (Distribution<'s * float<reward>>) encodes both the set 's and the transition probabilities (including the reward)
    // The MDP models the environment. Although the reward system can be part of the same robot f.i. or organism it is8i also considered external to the agent
    // and part of the environment
    public delegate Distribution<(S, Reward)> Dynamics<S, A>(S state, A action);

    // A policy is a mapping from states to the probabilities of selecting each possible action
    public delegate Distribution<A> Policy<S, A>(S state);

    // γ = discount rate

}
