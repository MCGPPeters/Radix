using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Learning.Reinforced
{


    /// <summary>
    ///     Action value
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public record Q<A>(Random<A> Value) : Expectation<A>(Value);


}
