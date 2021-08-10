using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;
using Xunit;
using Radix.Validated;
using System;
using static Radix.Math.Applied.Probability.Distribution.Generators;
using static Radix.Math.Applied.Learning.Reinforced.MonteCarlo.Prediction.FirstVisit;
using XPlot.Plotly;
using System.Diagnostics.CodeAnalysis;
using Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_4;

namespace Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_5
{
    public interface Action
    {
        static Action Hit() => new Hit();

        static Action Stick() => new Stick();
    };

    public record Hit : Action;
    public record Stick : Action;

    public record Deck() :
        Alias<Distribution<int>>(Distribution<int>.
            Uniform(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 }));

    public record Card(Deck deck) : Alias<Random<int>>(deck.Value.Choose());
    public record Hand(Deck deck) : Alias<List<Card>>(new List<Card> { new Card(deck), new Card(deck) });

    public class State : IEquatable<State?>
    {
        public Hand PlayerHand { get; internal set; }
        public Hand DealerHand { get; internal set; }
        public bool GameOver { get; internal set; }
        public bool DealerPlayed { get; internal set; }
        public override bool Equals(object? obj) => Equals((State)obj);
        public bool Equals(State? other) => EqualityComparer<Hand>.Default.Equals(PlayerHand, other.PlayerHand) && EqualityComparer<Card>.Default.Equals(DealerHand.Value[0], other.DealerHand.Value[0]);
        public override int GetHashCode() => HashCode.Combine(PlayerHand, DealerHand.Value[0]);
}

    public record Observation()
    {
        public Hand PlayerHand { get; internal set; }
        public bool GameOver { get; internal set; }
        public Hand DealerHand { get; internal set; }
        public bool DealerPlayed { get; internal set; }
    }

    public record BlackJack
    {
        public static (Environment<State, Action, Observation> environment, State initialState, Observation initialObservation) Create()
        {
            var result =
                from γ in DiscountFactor.Create(1.0)
                select new Environment<State, Action, Observation>(Dynamics(new Deck()), Reward, γ);
            Deck deck = new Deck();
            Hand dealerHand = new Hand(deck);
            Hand playerHand = new Hand(deck);
            State initialState = new State
            {
                DealerHand = dealerHand,
                PlayerHand = playerHand,
                DealerPlayed = false,
                GameOver = false
            };

            Observation initialObservation = new Observation
            {
                DealerHand = dealerHand,
                PlayerHand = playerHand,
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
                DealerHand = transition.Destination.DealerHand,
                PlayerHand = transition.Destination.PlayerHand,
                DealerPlayed = transition.Destination.DealerPlayed
            };
        };


        private static Func<Hand, bool> IsBust => hand => hand.Value.Sum(x => x.Value) > 21;

        private static Func<Deck, Hand, Hand> Draw => (deck, hand) =>
        {
            hand.Value.Add(new Card(deck));
            return hand;
        };

        private static Func<Hand, double> Score =>
            hand => IsBust(hand) ? 0 : hand.Value.Sum(x => x.Value);

        public static Func<Hand, bool> IsNatural =>
            hand =>
            hand.Value.Count() == 2
            && hand.Value.Count(x => x.Value == 1) == 1
            && hand.Value.Count(x => x.Value == 11) == 1;

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

        private static Func<Deck, Math.Applied.Optimization.Control.MDP.Dynamics<State, Action>> Dynamics = (deck) => (state, action) =>
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
                            GameOver = true
                        }
                        : new State
                        {
                            DealerPlayed = state.DealerPlayed,
                            DealerHand = state.DealerHand,
                            PlayerHand = newPlayerHand,
                            GameOver = false
                        };
                    return (new Transition<State>(state, nextState), new Probability(1.0));

                case Stick:
                    Hand newDealerHand;
                    while (state.DealerHand.Value.Sum(x => x.Value) < 17)
                        newDealerHand = Draw(deck, state.DealerHand);

                    nextState = new State
                    {
                        DealerHand = state.DealerHand,
                        PlayerHand = state.PlayerHand,
                        GameOver = true,
                        DealerPlayed = true
                    };

                    return (new Transition<State>(state, nextState), new Probability(1.0));
                default:
                    throw new InvalidOperationException("Illegal move");
            }

        };
    }

    public class Example_5
    {
        [Fact(DisplayName = "First visit monte carlo prediction")]
        public void Test1()
        {
            var blackjack = BlackJack.Create();
            Policy<Observation, Action> π =
                s => s.PlayerHand.Value.Sum(x => x.Value) == 21 || s.PlayerHand.Value.Sum(x => x.Value) == 20
                ? Certainly(Action.Stick())
                : Certainly(Action.Hit());

            var stateValues = π.Evaluate(blackjack.environment, RunEpisode, 10000);

            var a = (from stateValue in stateValues
                     group stateValue by new { dealerCardValue = stateValue.Key.DealerHand.Value.First().Value.Value, PlayerSum = stateValue.Key.PlayerHand.Value.Sum(c => c.Value) } into grouping
                     select grouping
                     );

            double[,] z = new double[11,30];

            foreach (var grouping in a)
            {
                z[grouping.Key.dealerCardValue, grouping.Key.PlayerSum] =  grouping.Average(x => x.Value) ;             
            }

            var surface = new Surface
            {
                z = z
            };

            Chart.Plot(surface).Show();

            var stateValues_ = π.Evaluate(blackjack.environment, RunEpisode, 500000);

            var a_ = (from stateValue in stateValues_
                      group stateValue by new { dealerCardValue = stateValue.Key.DealerHand.Value.First().Value.Value, PlayerSum = stateValue.Key.PlayerHand.Value.Sum(c => c.Value) } into grouping
                      select grouping
                     );

            var z_ = new double[11, 30];

            foreach (var grouping in a_)
            {
                z_[grouping.Key.dealerCardValue, grouping.Key.PlayerSum] = grouping.Average(x => x.Value);
            }

            var surface_ = new Surface
            {
                z = z_
            };

            Chart.Plot(surface_).Show();
        }

        private static Func<Policy<Observation, Action>, List<(Transition<State>, Reward)>> RunEpisode = policy =>
        {
            var blackjack = BlackJack.Create();
            var rewards = new List<(Transition<State>, Reward)>();
            var s = blackjack.initialState;
            var o = blackjack.initialObservation;
            var gameOver = false;
            while (!gameOver)
            {
                Action action = policy(o).Choose().Value;
                (var transition, var probability) = blackjack.environment.Dynamics(s, action);
                s = transition.Destination;
                gameOver = s.GameOver;
                o = o with
                {
                    DealerHand = s.DealerHand,
                    PlayerHand = s.PlayerHand,
                    GameOver = s.GameOver,
                    DealerPlayed = s.DealerPlayed
                };
                rewards.Add((transition, blackjack.environment.Reward(o)));
            }
            return rewards;
        };
    }
}
