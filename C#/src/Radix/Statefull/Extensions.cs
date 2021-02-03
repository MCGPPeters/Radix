//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radix.Statefull
//{
//    public static class Extensions
//    {
//        public static(V, S)  Run<S, V>(Statefull<S, V> statefull, S state)
//            => statefull(state);               

//        /// <summary>
//        /// Monadic bind
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="U"></typeparam>
//        /// <param name="prior"></param>
//        /// <param name="f"></param>
//        /// <returns></returns>
//        public static Statefull<S, VV> Bind<S, V, VV>(this Statefull<S, V> statefull, Func<V, Statefull<S, VV>> f)
//        {
//            (VV, S) Run(S state)
//            {
//                var (value, newState) = Extensions.Run(statefull, state);
//                var nextValue = f(value);
//                var nextState = nextValue(newState);
//                return Extensions.Run<S, VV>(nextState, nextValue);
//            }

//            return Run;

//        }

//        /// <summary>
//        /// For Linq support
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="U"></typeparam>
//        /// <param name="prior"></param>
//        /// <param name="f"></param>
//        /// <returns></returns>
//        public static Distribution<U> SelectMany<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f)
//            => prior.Bind(f);

//        /// <summary>
//        /// Functor map
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="U"></typeparam>
//        /// <param name="distribution"></param>
//        /// <param name="project"></param>
//        /// <returns></returns>
//        public static Distribution<U> Map<T, U>(this Distribution<T> distribution, Func<T, U> project)
//            => Distribution<T>.Map(distribution, project);

//        /// <summary>
//        /// for Linq support
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <typeparam name="U"></typeparam>
//        /// <param name="distribution"></param>
//        /// <param name="project"></param>
//        /// <returns></returns>
//        public static Distribution<U> Select<T, U>(this Distribution<T> distribution, Func<T, U> project)
//            => distribution.Map(project);
//    }
//}


