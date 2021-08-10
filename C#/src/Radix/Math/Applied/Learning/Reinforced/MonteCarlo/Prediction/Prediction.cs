using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Probability;
using static System.Math;

namespace Radix.Math.Applied.Learning.Reinforced.MonteCarlo.Prediction
{
    public static class FirstVisit
    {
        public static Dictionary<S, double> Evaluate<S, A, O>(this Policy<O, A> π, Environment<S, A, O> environment, Func<Policy<O, A>, List<(Transition<S>, Reward)>> runEpisode, int numberOfEpisodes) where S : notnull where A : notnull where O : notnull
        {
            var stateValues = new Dictionary<S, double>();
            var returns = new Dictionary<S, List<Return>>();
            var n = 0;
            while (n <= numberOfEpisodes)
            {
                var episode = runEpisode(π);
                var G = 0.0;
                var visits = new List<S>();
                foreach ((var transition, var reward) in episode)
                {
                    if (visits.Contains(transition.Origin)) continue;
                    visits.Add(transition.Origin);
                    G = environment.γ * G + reward.Value;
                    if (returns.TryGetValue(transition.Origin, out var rs))
                    {
                        rs.Add(new Return(G));
                    }
                    else
                    {
                        var xs = new List<Return>();
                        xs.Add(new Return(G));
                        returns.Add(transition.Origin, xs);
                    }

                    double avg = returns[transition.Origin].Average(x => x.Value);
                    stateValues[transition.Origin] = avg;
                }
                n++;
            }
            return stateValues;
        }
    }
}
