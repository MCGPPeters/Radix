using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Probability;
using Radix.Validated;
using static Radix.Math.Applied.Probability.Distribution.Generators;

namespace Radix.Math.Applied.Optimization.Control.POMDP
{
    public record Environment<State, Action, Observation>(MDP.Dynamics<State, Action> Dynamics, Reward<Observation> Reward, DiscountFactor γ);

    public delegate Reward Reward<Observation>(Observation observation);

    public delegate Action Agent<Observation, Action>(Reward<Observation> reward, Observation observation);

    public delegate Observation Reset<Observation>();

    public delegate Observation Step<State, Observation, Action>(Environment<State, Action, Observation> environment, State state, Action action);

    public delegate Unit Render<State, Action, Observation>(Environment<State, Action, Observation> environment);
}


