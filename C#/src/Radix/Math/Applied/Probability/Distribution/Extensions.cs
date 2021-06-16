using System;

namespace Radix.Math.Applied.Probability.Distribution
{
    public static class Extensions
    {
        /// <summary>
        ///     Monadic bind
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="prior"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Distribution<U> Bind<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull where T : notnull
            => Distribution<T>.Bind(prior, f);

        /// <summary>
        ///     For Linq support
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="prior"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Distribution<U> SelectMany<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull where T : notnull
            => prior.Bind(f);

        /// <summary>
        ///     Functor map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="distribution"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Distribution<U> Map<T, U>(this Distribution<T> distribution, Func<T, U> project) where U : notnull where T : notnull
            => Distribution<T>.Map(distribution, project);

        /// <summary>
        ///     for Linq support
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="distribution"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Distribution<U> Select<T, U>(this Distribution<T> distribution, Func<T, U> project) where U : notnull where T : notnull
            => distribution.Map(project);

        public static Probability Sum<T>(this Distribution<T> distribution) where T : notnull
            => Distribution<T>.Sum(distribution);

        public static Distribution<T> Scale<T>(this Distribution<T> distribution) where T : notnull
            => Distribution<T>.Scale(distribution);
    }
}
