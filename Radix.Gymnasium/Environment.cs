using System.Globalization;
using System.Reflection.Metadata;
using Python.Runtime;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Optimization.Control.POMDP;

namespace Radix.Gymnasium;

public static partial class Environment
{
    public interface Action;

    /// <summary>
    ///    This is the set of all possible states of the environment.
    ///    A state is the complete description of everything about the environment at a particular time step.
    ///    In a fully observable environment, the state is the same as the observation.
    ///    In the context of the Blackjack game (Radix.Tests), the state could include the dealer's cards, the player's cards,
    ///    and whether the game is over.
    ///    In the context of the CartPole game, the state could include the position and velocity of the cart,
    ///    and the angle and angular velocity of the pole.
    /// </summary>
    public interface State;

    /// <summary>
    ///    An observation is the information that the agent receives about the state of the environment.
    ///    Depending on the environment, an observation might not include all information about the state.
    ///    For example, in a partially observable environment, the agent might not be able to see all of the state's details.
    public interface Observation;

    public interface Name;

    public interface RenderMode;

    public static class BlackJack
    {
        public record State(int CurrentSum, int DealerCardValue, bool UsableAce) : Environment.State;

        public record Observation(int CurrentSum, int DealerCardValue, bool UsableAce) : Environment.Observation;
        
        public interface Action : Environment.Action;

        public record struct Hit : Action;

        public record struct Stick : Action;

        public static Action[] ActionSpace = [new Hit(), new Stick()];

        /// <summary>
        ///     Converts the Action to the corresponding type in the Gymnasium project
        /// </summary>
        /// <returns>
        ///     The corresponding type in the Gymnasium project
        /// </returns>
        public static dynamic ConvertAction(Action action)
        {
            return action switch
            {
                Stick => 0,
                Hit => 1,
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        ///    Converts the Gymnasium action to the corresponding type in the Radix project
        /// </summary>
        /// <returns></returns>
        public static Action ConvertAction(dynamic gymAction)
        {
            return gymAction switch
            {
                0 => new Stick(),
                1 => new Hit(),
                _ => throw new NotImplementedException()
            };
        }


        /// <summary>
        ///     Converts the Gymnasium observation to the corresponding type in the Radix project
        /// </summary>
        /// <returns>
        ///     The corresponding type in the Radix project
        /// </returns>
        public static Observation ConvertObservation(dynamic gymObservation)
        {
            PyTuple observationArray = gymObservation;
            int currentSum = observationArray[0].ToInt32(new NumberFormatInfo());
            int dealerCardValue = observationArray[1].ToInt32(new NumberFormatInfo());
            bool usableAce = Convert.ToBoolean(observationArray[2].ToInt32(new NumberFormatInfo()));
            return new Observation(currentSum, dealerCardValue, usableAce);
        }
        
        
    }
}
