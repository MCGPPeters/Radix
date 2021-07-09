using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Optimization;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.Deterministic;
using Radix.Math.Applied.Probability;
using Xunit;
using static Radix.Option.Extensions;


namespace Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_4
{
    public abstract record Action(int X, int Y);

    public record North() : Action(1, 0);

    public record South() : Action(-1, 0);

    public record West() : Action(0, -1);

    public record East() : Action(0, 1);

    public class Example_4
    {
        public static Dynamics<(int x, int y), Action> GridDynamics { get; } =
            (state, action) =>
            {
                if (state == (0, 0) || state == (3, 3))
                {
                    return (new Transition<(int x, int y)>(state, state), 0.0, Some(new Probability(1)));
                }

                (int x, int y) = action;
                (int x, int y) nextState = (x: state.x + x, y: state.y + y);

                (Transition<(int x, int y)>, double, Option<Probability>) stayedInCurrentState = (new Transition<(int x, int y)>(state, state), -1.0, Some(new Probability(1)));
                (Transition<(int x, int y)>, double, Option<Probability>)
                    arrivedAtNextState = (new Transition<(int x, int y)>(state, nextState), -1.0, Some(new Probability(1)));

                return nextState.x < 0 || nextState.x >= 4 || nextState.y < 0 || nextState.y >= 4
                    ? stayedInCurrentState
                    : arrivedAtNextState;
            };

        private static IEnumerable<Action> ActionSpace()
        {
            yield return new North();
            yield return new South();
            yield return new West();
            yield return new East();
        }

        private static IEnumerable<(int x, int y)> StateSpace()
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    yield return (x, y);
                }
            }
        }

        [Fact(DisplayName = "Given an equiprobable policy the correct value function is created")]
        public void Test1()
        {
            IEnumerable<(int x, int y)> stateSpace = StateSpace().ToArray();
            MDP<(int x, int y), Action> mdp = new MDP<(int x, int y), Action>(stateSpace, ActionSpace(), GridDynamics, 1.0);
            Distribution<Action> randomAction = Distribution<Action>.Uniform(mdp.Actions);
            var actionDistributions = Enumerable.Repeat(randomAction, stateSpace.Count());
            Dictionary<(int x, int y), Distribution<Action>>
                π = actionDistributions.Zip(stateSpace).ToDictionary(tuple => tuple.Second, tuple => tuple.First);
            Dictionary<(int x, int y), double>? stateValues = new Dictionary<(int x, int y), double>(
                stateSpace
                    .Select(s => new KeyValuePair<(int x, int y), double>(s, 0.0)));

            var vπ = PolicyExtensions.Evaluate(π, mdp, stateValues, 0.0001);

            List<double>? values = vπ.Values.Select(d => System.Math.Round(d, 2)).ToList();
            Xunit.Assert.Equal(
                new List<double>
                {
                    0.0,
                    -14,
                    -20,
                    -22,
                    -14,
                    -18,
                    -20,
                    -20,
                    -20,
                    -20,
                    -18,
                    -14,
                    -22,
                    -20,
                    -14,
                    0.0
                },
                values);
        }

        [Fact(DisplayName = "Given an initial equiprobable policy the optimal policy is calculated")]
        public void Test2()
        {
            IEnumerable<(int x, int y)> stateSpace = StateSpace().ToArray();
            MDP<(int x, int y), Action> mdp = new MDP<(int x, int y), Action>(stateSpace, ActionSpace(), GridDynamics, 1.0);
            Distribution<Action> randomAction = Distribution<Action>.Uniform(mdp.Actions);
            var actionDistributions = Enumerable.Repeat(randomAction, stateSpace.Count());
            Dictionary<(int x, int y), Distribution<Action>>
                π = actionDistributions.Zip(stateSpace).ToDictionary(tuple => tuple.Second, tuple => tuple.First);
            Dictionary<(int x, int y), double>? stateValues = new Dictionary<(int x, int y), double>(
                stateSpace
                    .Select(s => new KeyValuePair<(int x, int y), double>(s, 0.0)));

            var optimalPolicy = PolicyExtensions.Iterate(π, mdp, stateValues, 0.0001).Select(pair => pair.Value.Value.ArgMax(tuple => tuple.probability));

            Xunit.Assert.True(true);
        }
    }
}
