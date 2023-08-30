using Radix.Control.Validated;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;
using Xunit;
using Radix.Data;
using static Radix.Math.Applied.Probability.Distribution.Generators;
using static Radix.Math.Applied.Learning.Reinforced.MonteCarlo.Prediction.FirstVisit;
using XPlot.Plotly;

namespace Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_5
{
    public interface Action
    {
        static Action Hit() => new Hit();

        static Action Stick() => new Stick();
    };

    public record Hit : Action;
    public record Stick : Action;

    [Alias<Distribution<int>>]

    public partial record Deck 
    {
        public Deck() => Value = Distribution<int>.Uniform([ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 ]);
    }

    [Alias<Random<int>>]
    public partial record Card;

    public record Hand(List<Card> Cards);

    public class State : IEquatable<State>
    {
        public Card DealerShowing { get; set; }
        public int PlayerSum { get; set; }
        public Hand PlayerHand { get; internal set; }
        public Hand DealerHand { get; internal set; }

        public bool PlayerHasUsableAce { get; set; }
        public bool GameOver { get; internal set; }
        public bool DealerPlayed { get; internal set; }

        public override bool Equals(object? obj) => Equals((State)obj);
        public bool Equals(State? other)
        {
            bool v = EqualityComparer<int>.Default.Equals(PlayerSum, other.PlayerSum)
                        && EqualityComparer<Card>.Default.Equals(DealerShowing, other.DealerShowing)
                        && PlayerHasUsableAce == other.PlayerHasUsableAce;
            return v;
        }
        public override int GetHashCode()
        {
            int v = HashCode.Combine(PlayerSum, DealerShowing, PlayerHasUsableAce);
            return v;
        }

        public override string ToString() => $"DealerShowing : {DealerShowing}, PlayerSum : {PlayerSum}, PlayerHasUsableAce: {PlayerHasUsableAce}";
    }

    public record Observation
    {
        public Card DealerShowing { get; set; }
        public Hand PlayerHand { get; internal set; }
        public int PlayerSum { get; set; }
        public bool GameOver { get; internal set; }
        public bool DealerPlayed { get; internal set; }
        public Hand DealerHand { get; internal set; }
    }

    public record BlackJack
    {
        public static (Environment<State, Action, Observation> environment, State initialState, Observation initialObservation) Create()
        {
            var result =
                from γ in DiscountFactor.Create(1.0)
                select new Environment<State, Action, Observation>(Dynamics(new Deck()), Reward, γ);
            Deck deck = new Deck();
            Hand dealerHand = new Hand(new List<Card> { (Card)(deck.Value.Choose()), (Card)(deck.Value.Choose()) });
            Hand playerHand = new Hand(new List<Card> { (Card)(deck.Value.Choose()), (Card)(deck.Value.Choose()) });
            var playerSum = playerHand.Cards.Sum(x => x.Value);

            State initialState = new State
            {
                DealerShowing = dealerHand.Cards[0],
                PlayerSum = playerSum,
                DealerHand = dealerHand,
                PlayerHand = playerHand,
                DealerPlayed = false,
                GameOver = false,
                PlayerHasUsableAce = ContainsUsableAce(playerHand)
            };

            Observation initialObservation = new Observation
            {
                DealerShowing = dealerHand.Cards[0],
                DealerHand = dealerHand,
                PlayerHand = playerHand,
                PlayerSum = playerSum,
                DealerPlayed = false,
                GameOver = false
            };

            return result switch
            {
                Valid<Environment<State, Action, Observation>>(var valid) => (valid, initialState, initialObservation),
                _ => throw new InvalidOperationException()
            };
        }

        public static Step<State, Observation, Action> Step => (environment, state, action) =>
        {
            (var transition, var _) = environment.Dynamics(state, action);
            return new Observation
            {
                GameOver = transition.Destination.GameOver,
                PlayerSum = transition.Destination.PlayerSum,
                DealerShowing = transition.Destination.DealerShowing,
                DealerPlayed = transition.Destination.DealerPlayed,
                PlayerHand = transition.Destination.PlayerHand,
                DealerHand = transition.Destination.DealerHand
            };
        };

        private static Func<Hand, bool> ContainsUsableAce =>
            hand => hand.Cards.Contains((Card)(new Random<int>(1))) && hand.Cards.Sum(x => x.Value) + 10 <= 21;

        private static Func<Hand, bool> IsBust => hand => Sum(hand) > 21;

        private static Func<Deck, Hand, Hand> Draw => (deck, hand) =>
        {
            hand.Cards.Add((Card)(deck.Value.Choose()));
            return hand;
        };

        private static Func<Hand, int> Sum
            => hand
                => ContainsUsableAce(hand)
                    ? hand.Cards.Sum(x => x.Value) + 10
                    : hand.Cards.Sum(x => x.Value);

        private static Func<Hand, double> Score =>
            hand => IsBust(hand) ? 0 : hand.Cards.Sum(x => x.Value);

        public static Func<Hand, bool> IsNatural =>
            hand =>
            hand.Cards.Count() == 2
            && hand.Cards.Count(x => x.Value == 1) == 1
            && hand.Cards.Count(x => x.Value == 11) == 1;

        private static Reward<Observation> Reward => observation =>
        {
            if (observation.DealerPlayed)
            {
                var playerScore = Score(observation.PlayerHand);
                var dealerScore = Score(observation.DealerHand);

                var reward = Convert.ToDouble(playerScore > dealerScore) - Convert.ToDouble(playerScore < dealerScore);

                if (IsNatural(observation.DealerHand) && IsNatural(observation.PlayerHand) && EqualityComparer<double>.Default.Equals(reward, 1.0))
                    reward = 1.5;

                return new Reward(reward);
            }
            else
            {
                if (IsBust(observation.PlayerHand))
                {
                    return new Reward(-1.0);
                }

                return new Reward(0.0);
            }
        };

        private static Func<Deck, Math.Applied.Optimization.Control.MDP.Dynamics<State, Action>> Dynamics =
            (deck) =>
                (state, action) =>
                {
                    State nextState;
                    switch (action)
                    {
                        case Hit:
                            Hand newPlayerHand = Draw(deck, state.PlayerHand);
                            nextState = IsBust(newPlayerHand)
                                ? new State
                                {
                                    DealerPlayed = state.DealerPlayed,
                                    DealerHand = state.DealerHand,
                                    PlayerHand = newPlayerHand,
                                    DealerShowing = state.DealerShowing,
                                    PlayerSum = state.PlayerSum,
                                    GameOver = true,
                                    PlayerHasUsableAce = ContainsUsableAce(newPlayerHand)
                                }
                                : new State
                                {
                                    DealerPlayed = state.DealerPlayed,
                                    DealerHand = state.DealerHand,
                                    DealerShowing = state.DealerShowing,
                                    PlayerSum = state.PlayerSum,
                                    PlayerHand = newPlayerHand,
                                    GameOver = false,
                                    PlayerHasUsableAce = ContainsUsableAce(newPlayerHand)
                                };
                            return (new Transition<State>(state, nextState), (Probability)1);

                        case Stick:
                            Hand newDealerHand = state.DealerHand;
                            while (state.DealerHand.Cards.Sum(x => x.Value) < 17)
                                newDealerHand = Draw(deck, state.DealerHand);

                            nextState = new State
                            {
                                DealerHand = newDealerHand,
                                PlayerHand = state.PlayerHand,
                                DealerShowing = state.DealerShowing,
                                PlayerSum = state.PlayerSum,
                                GameOver = true,
                                DealerPlayed = true,
                                PlayerHasUsableAce = ContainsUsableAce(state.PlayerHand)
                            };

                            return (new Transition<State>(state, nextState), (Probability)1);
                        default:
                            throw new InvalidOperationException("Illegal move");
                    }

                };
    }



    public class Example_5
    {
        private static Func<Policy<Observation, Action>, List<(Transition<State>, Reward)>> RunEpisode =
          policy =>
          {
              var (environment, initialState, initialObservation) = BlackJack.Create();
              var rewards = new List<(Transition<State>, Reward)>();
              var s = initialState;
              var o = initialObservation;
              var gameOver = false;
              while (!gameOver)
              {
                  Action action = policy(o).Choose().Value;
                  (var transition, var probability) = environment.Dynamics(s, action);
                  s = transition.Destination;
                  gameOver = s.GameOver;
                  o = o with
                  {
                      DealerHand = s.DealerHand,
                      PlayerHand = s.PlayerHand,
                      GameOver = s.GameOver,
                      DealerPlayed = s.DealerPlayed,
                      PlayerSum = s.PlayerSum,
                      DealerShowing = s.DealerShowing
                  };
                  rewards.Add((transition, environment.Reward(o)));
              }
              return rewards;
          };

        [Fact(DisplayName = "First visit monte carlo prediction", Skip = "True")]
        public void Test1()
        {
            var blackjack = BlackJack.Create();
            Policy<Observation, Action> π =
                s => s.PlayerHand.Cards.Sum(x => x.Value) == 21 || s.PlayerHand.Cards.Sum(x => x.Value) == 20
                ? Certainly(Action.Stick())
                : Certainly(Action.Hit());

            var stateValues = π.Evaluate(blackjack.environment, RunEpisode, 10000);
            PlotValueFunction(stateValues);

            //var stateValues_ = π.Evaluate(blackjack.environment, RunEpisode, 500000);
            //PlotValueFunction(stateValues_);
        }

        private static void PlotValueFunction(Expectation<State> stateValue)
        {
            var zWithUsableAce = [12, 22];
            var zWithoutUsableAce = [12.0, 22.0];

            for (int i = 1; i < 12; i++)
            {
                for (int j = 12; j < 22; j++)
                {
                    State stateWithUsableAce =
                        new State
                        {
                            DealerShowing = (Card)(new Random<int>(i)),
                            PlayerSum = j,
                            PlayerHasUsableAce = true
                        };
                    zWithUsableAce[i, j] = stateValue(new Random<State>(stateWithUsableAce));

                    State stateWithoutUsableAce =
                        new State
                        {
                            DealerShowing = (Card)(new Random<int>(i)),
                            PlayerSum = j,
                            PlayerHasUsableAce = false
                        };
                    zWithoutUsableAce[i, j] = stateValue(new Random<State>(stateWithoutUsableAce));
                }
            }

            var surfaceWithUsableAce = new Surface
            {
                z = zWithUsableAce,
                name = "With usable ace"
            };

            Chart.Plot(surfaceWithUsableAce).Show();

            var surfaceWithoutUsableAce = new Surface
            {
                z = zWithoutUsableAce,
                name = "Without usable ace"
            };

            Chart.Plot(surfaceWithoutUsableAce).Show();
        }


    }
}
