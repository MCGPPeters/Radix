using Radix.Math.Applied.Optimization;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Learning.Reinforced.TemporalDifference.Control.OffPolicy
{
    public interface Agent<T, Observation, Action>
    {
        static abstract Policy<Observation, Action> Act(T t);
        static abstract Learn<T, Observation, Action> Learn (T t);

    }


    public class QLearningAgent<Observation, Action> : Agent<QLearning<Observation, Action>, Observation, Action>
    {
        public static Learn<QLearning<Observation, Action>, Observation, Action> Learn(QLearning<Observation, Action> t) => 
            (observation, action, reward, nextObservation) =>
            {
                var currentQValue = new Expectation<Return>(new Random<Return>((Return)0.0));
                if (t.QTable.TryGetValue((observation, action), out var qValue))
                {
                    currentQValue = qValue;
                }
                var actionWithMaxQValue = t.QTable.Any() 
                    ? t.QTable.MaxBy(kvp => kvp.Value, new ExpectationComparer()).Key.Action 
                    : default;
                var maxNextQValue = new Expectation<Return>(new Random<Return>((Return)0.0));
                if (t.QTable.TryGetValue((nextObservation, actionWithMaxQValue), out var nextQValue))
                {
                    maxNextQValue = nextQValue;
                }
                var δt = reward + t.γ.Value * maxNextQValue.Value.Value - currentQValue.Value.Value;
                Expectation<Return> qNext = new(
                    new Random<Return>(
                        (Return)(currentQValue.Value.Value + t.α.Value * δt)
                    )
                );
                t.QTable[(observation, action)] = qNext;

                return t with 
                {
                    ε = ExplorationRate.Create(System.Math.Max(t.minimalExplorationRate, t.ε - t.explorationRateDecay)) switch
                    {
                        Invalid<ExplorationRate> invalid => throw new NotImplementedException(),
                        Valid<ExplorationRate> (var valid) => valid,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    TrainingErrors = [.. t.TrainingErrors, (TrainingError)δt],
                    Rewards = [.. t.Rewards, reward]
                };
            };

        public static Policy<Observation, Action> Act(QLearning<Observation, Action> t) => Policy.εGreedy<Observation, Action>(
            o => a => 
            {
                if (!t.QTable.ContainsKey((o, a)))
                {
                    t.QTable[(o, a)] = new Expectation<Return>(new Random<Return>((Return)0));
                }
                return t.QTable[(o, a)];
            }, t.ε, t.ActionSpace);
    }

    public readonly record struct QLearning<Observation, Action>(Dictionary<(Observation Observation, Action Action) , Expectation<Return>> QTable, DiscountFactor γ, LearningRate α, ExplorationRate ε, ExplorationRate minimalExplorationRate, double explorationRateDecay, Action[] ActionSpace, TrainingError[] TrainingErrors, Reward[] Rewards) where Observation : notnull where Action : notnull;

    public delegate T Learn<T, Observation, Action>(Observation observation, Action action, Reward reward, Observation nextObservation);

    // public static class QLearninga
    // {


    //     public static Action Act<Observation, Action>(Policy<Observation, Action> π, Q<Observation, Action> q, Observation observation) =>
    //         π(observation).Choose();

    //     public static Agent<Observation, (Action, TrainingError)> Agent<Observation, Action>(DiscountFactor γ, LearningRate α, ExplorationRate ε, ExplorationRate minimalExplorationRate, double explorationRateDecay,  Action[] actionSpace)
    //         where Action : notnull
    //         where Observation : notnull
    //     {
    //         var qTable = new Dictionary<Observation, Dictionary<Action, Expectation<Return>>>();
    //         Q<Observation, Action> q = o => a => 
    //         {
    //             if (!qTable.ContainsKey(o))
    //             {
    //                 qTable[o] = new Dictionary<Action, Expectation<Return>>();
    //             }
    //             if (!qTable[o].ContainsKey(a))
    //             {
    //                 qTable[o][a] = new Expectation<Return>(new Random<Return>((Return)0));
    //             }
    //             return qTable[o][a];
    //         };
    //         return (reward, observation) =>
    //         {
    //             // Choose action from observation using policy derived from Q
    //             var π = Policy.εGreedy(q, ε, actionSpace);
    //             var actionDistribution = π(observation);
    //             var action = actionDistribution.Choose();

    //             var currentQValue = q(observation)(action);

    //             var maxNextQValueOutcome = actionDistribution.OutcomeProbabilities
    //                 .MaxBy(op => q(observation)(op.outcome), new ExpectationComparer())
    //                 .outcome;
    //             var maxNextQValue = q(observation)(maxNextQValueOutcome);

    //             // the TD error
    //             var δt = reward + γ.Value * maxNextQValue.Value.Value - currentQValue.Value.Value;

    //             Expectation<Return> qNext = new(
    //                 new Random<Return>(
    //                     (Return)(currentQValue.Value.Value + α.Value * δt)
    //                 )
    //             );
    //             qTable[observation][action] = qNext;

    //             ε = ExplorationRate.Create(System.Math.Max(minimalExplorationRate, ε - explorationRateDecay)) switch
    //             {
    //                 Invalid<ExplorationRate> invalid => throw new NotImplementedException(),
    //                 Valid<ExplorationRate> (var valid) => valid,
    //                 _ => throw new ArgumentOutOfRangeException()
    //             };

    //             return (action, (TrainingError)δt);
    //         };
    //     }      
    // }

    [Alias<double>]
    public readonly partial record struct TrainingError
    {
    }

    public class ExpectationComparer : IComparer<Expectation<Return>>
    {
        public int Compare(Expectation<Return> x, Expectation<Return> y){
            if (x.Value.Value > y.Value.Value)
            {
                return 1;
            }
            if (x.Value.Value < y.Value.Value)
            {
                return -1;
            }
            return 0;
        }
    }
}
