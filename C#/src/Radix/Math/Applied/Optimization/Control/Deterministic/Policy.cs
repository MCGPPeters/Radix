using System.Reactive.Linq;
using Radix.Math.Applied.Optimization.Control.MDP;
using Radix.Math.Applied.Probability;
using Radix.Math.Applied.Probability.Distribution;
using Radix.Option;
using static System.Math;

namespace Radix.Math.Applied.Optimization.Control.Deterministic;

public static class Policy
{
    /// <summary>
    /// Test how good a policy is. Do this by, for each state in the state space, calculating the value of being in that space when following policy π
    /// In each state, for each action:
    /// - calculate how much reward you would get when following that action
    /// - then get the probability that state transition actually occurs and weigh it by the probability the agent actually takes the action (given that it still can taken each action given its probability)
    /// - use that probability to calculate the expected return when taking the action in this state under policy π
    /// - add the return to the total value of state S (which is the maximum reward one could get in this state following policy π from this state onward)
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="mdp"></param>
    /// <param name="stateValues"></param>
    /// <param name="θ"></param>
    /// <returns></returns>
    public static Dictionary<S, double> Evaluate<S, A>(this Policy<S, A> π, Environment<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull where A : notnull
    {
        double Δ = 0.0;
        do
        {
            Δ = 0.0;
            foreach (S state in mdp.States)
            {
                double oldValue = stateValues[state];
                double value = 0.0;

                foreach (A action in mdp.Actions)
                {
                    (Transition<S> transition, Probability.Probability transitionProbability) = mdp.Dynamics(state, action);
                    Probability.Probability actionProbability = π(state).EventProbabilities.FirstOrDefault(eventProbability => eventProbability.@event.Value.Equals(action)).probability;
                    value += actionProbability * transitionProbability * (mdp.Reward(transition.Origin) + mdp.γ * stateValues[transition.Destination]);
                }

                stateValues[state] = value;
                Δ = Max(Δ, Abs(oldValue - value));
            }
        } while (Δ >= θ);

        return stateValues;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="π"></param>
    /// <param name="mdp"></param>
    /// <param name="stateValues"></param>
    /// <param name="θ"></param>
    /// <returns></returns>
    public static Policy<S, A> Improve<S, A>(Environment<S, A> mdp, Dictionary<S, double> stateValues) where S : notnull where A : notnull
    {
        Dictionary<S, Distribution<A>> π_ = new Dictionary<S, Distribution<A>>();

        foreach (S? state in mdp.States)
        {
            // select the actions actually yielding the most reward as a distribution
            IList<A>? bestActions = mdp.Actions.ArgMax(
                action =>
                {
                    (Transition<S> transition, Probability.Probability transitionProbability) = mdp.Dynamics(state, action);
                    return transitionProbability * (mdp.Reward(transition.Origin) + mdp.γ * stateValues[transition.Destination]);
                });
            π_[state] = Distribution<A>.Uniform(bestActions);
        }


        return s => π_[s];
    }

    public static Policy<S, A> Iterate<S, A>(Policy<S, A> π, Environment<S, A> mdp, Dictionary<S, double> stateValues, double θ) where S : notnull where A : notnull
    {
        bool isPolicyStable = false;
        Policy<S, A>? currentPolicy = π;


        while (!isPolicyStable)
        {
            Evaluate(currentPolicy, mdp, stateValues, 0.001);

            Policy<S, A>? newPolicy = Improve(mdp, stateValues);

            IEnumerable<A> newPolicyActions = stateValues.Keys.SelectMany(s => newPolicy(s).EventProbabilities.Select(tuple => tuple.@event.Value));
            IEnumerable<A> currentPolicyActions = stateValues.Keys.SelectMany(s => currentPolicy(s).EventProbabilities.Select(tuple => tuple.@event.Value));
            isPolicyStable = newPolicyActions.SequenceEqual(currentPolicyActions);

            currentPolicy = newPolicy;
        }


        return currentPolicy;
    }
}
