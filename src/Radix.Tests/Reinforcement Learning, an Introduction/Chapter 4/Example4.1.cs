//using Radix.Control.Validated;
//using Radix.Math.Applied.Optimization;
//using Radix.Math.Applied.Optimization.Control;
//using Radix.Math.Applied.Optimization.Control.MDP;
//using Radix.Math.Applied.Optimization.Control.Deterministic;
//using Radix.Math.Applied.Probability;
//using Xunit;
//using Radix.Data;

//namespace Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_4;

//public abstract record Action(int X, int Y);

//public record North() : Action(1, 0);

//public record South() : Action(-1, 0);

//public record West() : Action(0, -1);

//public record East() : Action(0, 1);

//public class Example_4
//{
//    private static Dynamics<State, Action> GridDynamics { get; } =
//        (state, action) =>
//        {
//            if (state == (0, 0) || state == (3, 3))
//            {
//                return (new Transition<State>(state, state), (Probability)1);
//            }

//            (int x, int y) = action;
//            State nextState = (x: state.x + x, y: state.y + y);

//            (Transition<State>, Probability) stayedInCurrentState = (new Transition<State>(state, state), (Probability)1);
//            (Transition<State>, Probability)
//                arrivedAtNextState = (new Transition<State>(state, nextState), (Probability)1);

//            return nextState.x < 0 || nextState.x >= 4 || nextState.y < 0 || nextState.y >= 4
//                ? stayedInCurrentState
//                : arrivedAtNextState;
//        };

//    private static Reward<State> Reward = s => (s == (0, 0) || s == (3, 3)) ? 0.0 : -1.0;

//    private static IEnumerable<Action> ActionSpace()
//    {
//        yield return new North();
//        yield return new South();
//        yield return new West();
//        yield return new East();
//    }

//    private static IEnumerable<State> StateSpace()
//    {
//        for (int x = 0; x < 4; x++)
//        {
//            for (int y = 0; y < 4; y++)
//            {
//                yield return new State(x, y);
//            }
//        }
//    }

//    [Fact(DisplayName = "Given an equiprobable policy the correct value function is created", Skip = "Will not run in linux container.. TODO")]
//    public void Test1()
//    {
//        IEnumerable<State> stateSpace = StateSpace().ToArray();

//        var validatedValues =
//            from γ in DiscountFactor.Create(1.0)
//            let environment = new Environment<State, Action>(stateSpace, ActionSpace(), GridDynamics, Reward, γ)
//            let π = new Policy<State, Action>(s => Distribution<Action>.Uniform(environment.Actions))
//            let stateValues = new Dictionary<State, double>(stateSpace.Select(s => new KeyValuePair<State, double>(s, 0.0)))
//            let vπ = π.Evaluate(environment, stateValues, 0.0001)
//            let values = vπ.Values.Select(d => System.Math.Round(d, 2)).ToList()
//            select values;
//        switch (validatedValues)
//        {
//            case Valid<List<double>>(var values):
//                Xunit.Assert.Equal(
//                    [
//                        0.0,
//                        -14,
//                        -20,
//                        -22,
//                        -14,
//                        -18,
//                        -20,
//                        -20,
//                        -20,
//                        -20,
//                        -18,
//                        -14,
//                        -22,
//                        -20,
//                        -14,
//                        0.0
//                    ],
//                    values);
//                break;

//            default:
//                Assert.Fail();
//                break;
//        }
//    }
//    [Fact(DisplayName = "Given an initial equiprobable policy the optimal policy is calculated")]
//    public void Test2()
//    {
//        IEnumerable<State> stateSpace = StateSpace().ToArray();
//        var optimalPolicy =
//            from γ in DiscountFactor.Create(1.0)
//            let environment = new Environment<State, Action>(stateSpace, ActionSpace(), GridDynamics, Reward, γ)
//            let π = new Policy<State, Action>(s => Distribution<Action>.Uniform(environment.Actions))
//            let stateValues = new Dictionary<State, double>(stateSpace.Select(s => new KeyValuePair<State, double>(s, 0.0)))
//            select Policy.Iterate(π, environment, stateValues, 0.0001);

//        switch (optimalPolicy)
//        {
//            case Valid<Policy<State, Action>>(var p):
//                var result = stateSpace.Select(s => p(s).OutcomeProbabilities.MaxBy(x => x.probability));
//                break;
//            default:
//                break;
//        }



//        Assert.Pass();
//    }
//}

//internal readonly record struct State(int x, int y)
//{
//    public static implicit operator (int x, int y)(State value)
//    {
//        return (value.x, value.y);
//    }

//    public static implicit operator State((int x, int y) value)
//    {
//        return new State(value.x, value.y);
//    }
//}
