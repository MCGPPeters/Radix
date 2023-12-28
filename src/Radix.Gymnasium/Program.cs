using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using Radix.Math.Applied.Optimization.Control;
using static Radix.Math.Applied.Learning.Reinforced.TemporalDifference.Control.OffPolicy.QLearning;
using static Radix.Control.Validated.Extensions;
using Python.Runtime;
using System.Security.Cryptography;
using Radix.Data;
using static Radix.Gymnasium.Environment;

namespace Radix.Gymnasium
{
    class Program
    {
        enum Algorithm
        {
            QLearning
        }


        static int Main(string[] args)
        {
            var rootCommand = new RootCommand("Gymnasium command-line app");

            System.CommandLine.Option<string> environmentOption = new("--environment", "The gymnasium environment");
            System.CommandLine.Option<Algorithm> agentOption = new("--algorithm", "The type of algorithm");
            System.CommandLine.Option<int> episodeOption = new("--episodes", "The number of episodes to run");
            var runCommand = new Command("run", "Run the gymnasium")
                                            {
                                                environmentOption,
                                                agentOption,
                                                episodeOption
                                            };

            rootCommand.AddCommand(runCommand);

            runCommand.SetHandler(RunGymnasium, environmentOption, agentOption, episodeOption);

            return rootCommand.Invoke(args);
        }


        private static Task RunGymnasium(string environment, Algorithm algorithm, int episodes)
        {
            Runtime.PythonDLL = "C:\\Users\\Maurice\\AppData\\Local\\Programs\\Python\\Python311\\python311.dll";
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();

            using (Py.GIL()) // Added missing curly braces
            {


                dynamic gym = Py.Import("gymnasium");
                dynamic env = gym.make(environment, render_mode: "human");
                

                

                switch (algorithm)
                {
                    case Algorithm.QLearning:
                        {
                            var rlAgent =
                                from γ in DiscountFactor.Create(0.99)
                                from α in LearningRate.Create(0.1)
                                from ε in ExplorationRate.Create(1.0)
                                from minimalExplorationRate in ExplorationRate.Create(0.1)
                                let explorationRateDecay = ε / (episodes / 2)
                                select Agent<BlackJack.Observation, BlackJack.Action>(γ, α, ε, minimalExplorationRate, explorationRateDecay, BlackJack.ActionSpace);

                            var reward = (Reward)0.0;
                            switch (rlAgent)
                            {
                                case Valid<Math.Applied.Optimization.Control.POMDP.Agent<BlackJack.Observation,
                                    BlackJack.Action>>(var validAgent):
                                    {
                                        for (int i = 0; i < episodes; i++)
                                        {
                                            dynamic initialObservation = env.reset();
                                            
                                            var done = false;
                                            BlackJack.Observation observation = BlackJack.ConvertObservation(initialObservation[0]);

                                            while (!done)
                                            {
                                                dynamic _ = env.render();
                                                dynamic? action =
                                                    BlackJack.ConvertAction(validAgent(reward, observation));
                                                PyTuple step = env.step(action);
                                                observation = BlackJack.ConvertObservation(step[0]);
                                                reward = (Reward)step[1].ToSingle(new NumberFormatInfo());
                                                if(reward > 0) Console.WriteLine(reward);
                                                done = Convert.ToBoolean(step[2]) || Convert.ToBoolean(step[3]);
                                            }
                                        }

                                        break;
                                    }
                                case Invalid<Math.Applied.Optimization.Control.POMDP.Agent<BlackJack.Observation
                                    , BlackJack.Action>> invalidAgent:
                                    {
                                        throw new NotImplementedException();
                                    }
                            }


                        }
                        break;

                    default:
                        {
                            throw new NotImplementedException();
                        }

                }

                return Task.CompletedTask;
            }
        }
    }
}

