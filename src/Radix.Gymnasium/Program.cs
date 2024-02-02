using System.CommandLine;
using System.Globalization;
using Radix.Math.Applied.Optimization.Control;
using static Radix.Control.Validated.Extensions;
using Python.Runtime;
using Radix.Data;
using static Radix.Gymnasium.Environment;
using Radix.Math.Applied.Learning.Reinforced.TemporalDifference.Control.OffPolicy;
using Radix.Math.Applied.Probability;

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
                dynamic plt = Py.Import("matplotlib.pyplot");
                dynamic patch = Py.Import("matplotlib.patches");
                dynamic tqdm = Py.Import("tqdm");
                dynamic np = Py.Import("numpy");
                dynamic seaborn = Py.Import("seaborn");

                dynamic env = gym.make(environment, render_mode: "human");
                env = gym.wrappers.RecordEpisodeStatistics(env);


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
                                select new QLearning<BlackJack.Observation, BlackJack.Action>([], γ, α, ε, minimalExplorationRate, explorationRateDecay, BlackJack.ActionSpace, [], []);

                            var reward = (Reward)0.0;
                            switch (rlAgent)
                            {
                                case Valid<QLearning<BlackJack.Observation, BlackJack.Action>>(var qLearning):
                                    {
                                        for (int i = 0; i < episodes; i++)
                                        {
                                            dynamic initialObservation = env.reset();
                                            
                                            var done = false;
                                            BlackJack.Observation observation = BlackJack.ConvertObservation(initialObservation[0]);

                                            while (!done)
                                            {
                                                dynamic _ = env.render();
                                                var action1 = QLearningAgent<BlackJack.Observation, BlackJack.Action>.Act(qLearning)(observation).Choose();
                                                dynamic? action =
                                                    BlackJack.ConvertAction(action1.Value);
                                                PyTuple step = env.step(action);
                                                var next_observation = BlackJack.ConvertObservation(step[0]);
                                                reward = (Reward)step[1].ToSingle(new NumberFormatInfo());
                                                qLearning = QLearningAgent<BlackJack.Observation, BlackJack.Action>.Learn(qLearning)(observation, action1.Value, reward, next_observation);
                                                done = Convert.ToBoolean(step[2].ToInt32(new NumberFormatInfo()));
                                            }
                                        }

                                        int rolling_length = 500;
                                        var fig = plt.subplots(ncols: 3, figsize: new[] { 12, 5 });
                                        var axs = fig[1];

                                        axs[0].set_title("Episode rewards");
                                        var reward_moving_average = (np.convolve(np.array(qLearning.Rewards.Select(reward => reward.Value).Cast<double>().ToArray()), np.ones(rolling_length), mode: "valid") / rolling_length);
                                        axs[0].plot(np.arange(reward_moving_average.shape[0]), reward_moving_average);

                                        axs[1].set_title("Episode lengths");
                                        var length_moving_average = np.convolve(np.array(env.length_queue).flatten(), np.ones(rolling_length), mode: "same") / rolling_length;
                                        axs[1].plot(np.arange(length_moving_average.shape[0]), length_moving_average);

                                        axs[2].set_title("Training Error");
                                        var training_error_moving_average = np.convolve(qLearning.TrainingErrors.Select(error => error.Value).Cast<double>().ToArray(), np.ones(rolling_length), mode: "same") / rolling_length;
                                        axs[2].plot(np.arange(training_error_moving_average.shape[0]), training_error_moving_average);

                                        plt.tight_layout();
                                        plt.show();

                                        break;
                                    }
                                case Invalid<QLearning<BlackJack.Observation, BlackJack.Action>> invalidAgent:
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

