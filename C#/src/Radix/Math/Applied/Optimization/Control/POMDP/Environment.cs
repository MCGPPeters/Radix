using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Probability;
using Radix.Validated;
using static Radix.Math.Applied.Probability.Distribution.Generators;

namespace Radix.Math.Applied.Optimization.Control.POMDP
{
    public record Environment<State, Action, Observation>(MDP.Dynamics<State, Action> Dynamics, Reward<Observation> Reward, DiscountFactor γ);

    public delegate double Reward<Observation>(Observation observation);

    public delegate Action Agent<Observation, Action>(Reward<Observation> reward, Observation observation);

    public delegate Observation Reset<Observation>();

    public delegate Observation Step<Observation>(Environment<State, Action, Observation> environment, State statem, Action action);

    public delegate Unit Render<State, Action, Observation>(Environment<State, Action, Observation> environment);

    public abstract record Action;

    public record Hit : Action;
    public record Stick : Action;

    public record Deck() :
        Alias<Distribution<int>>(Distribution<int>.
            Uniform(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 }));

    public record Card(Deck deck) : Alias<Random<int>>(deck.Value.Choose());
    public record Hand(Deck deck) : Alias<List<Card>>(new List<Card> { new Card(deck), new Card(deck) });

    public record State()
    {
        public Hand PlayerHand { get; internal set; }
        public Deck Deck { get; internal set; }
        public Hand DealerHand { get; internal set; }
        public bool GameOver { get; internal set; }
        public bool DealerPlayed { get; internal set; }
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
        public static (Environment<State, Action, Observation>, State initialState) Create()
        {
            var result =
                from γ in DiscountFactor.Create(1.0)
                select new Environment<State, Action, Observation>(Dynamics, Reward, γ);
            Deck deck = new Deck();
            State initialState = new State
            {
                Deck = deck,
                DealerHand = new Hand(deck),
                PlayerHand = new Hand(deck),
                DealerPlayed = false,
                GameOver = false
            };

            return result switch
            {
                Valid<Environment<State, Action, Observation>>(var valid) => (valid, initialState),
                _ => throw new InvalidOperationException()
            };
        }

        public static Step<Observation> Step => (environment, state, action) =>
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

                return reward;
            }
            else
            {
                if (IsBust(observation.PlayerHand))
                {
                    return -1.0;
                }

                return 0.0;
            }
        };

        private static MDP.Dynamics<State, Action> Dynamics = (state, action) =>
        {
            State nextState;
            switch (action)
            {
                case Hit:
                    Hand newPlayerHand = Draw(state.Deck, state.PlayerHand);
                    nextState = IsBust(newPlayerHand)
                        ? state with
                        {
                            DealerHand = state.DealerHand,
                            PlayerHand = newPlayerHand,
                            GameOver = true
                        }
                        : state with
                        {
                            DealerHand = state.DealerHand,
                            PlayerHand = newPlayerHand,
                            GameOver = false
                        };
                    return (new Transition<State>(state, nextState), new Probability.Probability(1.0));

                case Stick:
                    Hand newDealerHand;
                    while (state.DealerHand.Value.Sum(x => x.Value) < 17)
                        newDealerHand = Draw(state.Deck, state.DealerHand);

                    nextState = state with
                    {
                        GameOver = true,
                        DealerPlayed = true
                    };

                    return (new Transition<State>(state, nextState), new Probability.Probability(1.0));
                default:
                    throw new InvalidOperationException("Illegal move");
            }

        };
    }

}


