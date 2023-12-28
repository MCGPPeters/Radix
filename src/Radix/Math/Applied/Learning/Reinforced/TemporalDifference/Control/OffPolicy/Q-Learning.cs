using Radix.Math.Applied.Optimization;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Learning.Reinforced.TemporalDifference.Control.OffPolicy
{
    public static class QLearning
    {
        public static Action Decide<Observation, Action>(Policy<Observation, Action> π, Q<Observation, Action> q, Observation observation) =>
            π(observation).Choose();

        public static Agent<Observation, Action> Agent<Observation, Action>(DiscountFactor γ, LearningRate α, ExplorationRate ε, ExplorationRate minimalExplorationRate, double explorationRateDecay,  Action[] actionSpace)
            where Action : notnull
            where Observation : notnull
        {
            var qTable = new Dictionary<Observation, Dictionary<Action, Expectation<Return>>>();
            Q<Observation, Action> q = o => a => 
            {
                if (!qTable.ContainsKey(o))
                {
                    qTable[o] = new Dictionary<Action, Expectation<Return>>();
                }
                if (!qTable[o].ContainsKey(a))
                {
                    qTable[o][a] = new Expectation<Return>(new Random<Return>((Return)0));
                }
                return qTable[o][a];
            };
            return (reward, observation) =>
            {
                // Choose action from observation using policy derived from Q
                var π = Policy.εGreedy(q, ε, actionSpace);
                var actionDistribution = π(observation);
                var action = actionDistribution.Choose();

                
                var currentQValue = q(observation)(action);

                var maxNextQValueOutcome = actionDistribution.OutcomeProbabilities
                    .MaxBy(op => q(observation)(op.outcome), new ExpectationComparer())
                    .outcome;
                var maxNextQValue = q(observation)(maxNextQValueOutcome);

                Expectation<Return> qNext = new(
                    new Random<Return>(
                        (Return)(currentQValue.Value.Value + α.Value * (reward + γ.Value * maxNextQValue.Value.Value - currentQValue.Value.Value))
                    )
                );
                qTable[observation][action] = qNext;

                ε = ExplorationRate.Create(System.Math.Max(minimalExplorationRate, ε - explorationRateDecay)) switch
                {
                    Invalid<ExplorationRate> invalid => throw new NotImplementedException(),
                    Valid<ExplorationRate> (var valid) => valid,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return action;
            };
        }      
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
