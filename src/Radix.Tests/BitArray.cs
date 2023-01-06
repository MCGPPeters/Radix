using Radix.Math.Pure.Logic;
using Radix.Math.Pure.Numbers;

namespace Radix.Tests;

public record SDR
{
    public SDR(int length, int population)
    {
        int n = Length = length;
        int w = Population = population;
        Capacity = n! / (w! * (n - w)!);
    }

    public SDR(int length, Real sparsity)
    {
        Length = length;
        Population = (int)System.Math.Ceiling(length * sparsity);
    }

    public int Length { get; }
    public int Population { get; }
    public int Capacity { get; }
}

public static class BitArray
{
    /// <summary>
    /// Hamming weight, number of non zero elements
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    public static int Population(this System.Collections.BitArray bitArray)
    {
        int population = 0;
        for (int i = 0; i < bitArray.Count; i++)
        {
            if(bitArray[i] == true) population++; 
        }
        return population;
    }

    public static long Capacity(this System.Collections.BitArray bitArray, int sparsity = 100)
    {
        int w = bitArray.Population();
        int n = bitArray.Count;
        return n! / (w! * (n - w)!);
    }

    /// <summary>
    /// Determines if a bitarray is considered sparse compared to the <paramref name="upperPercentageLimit"/>
    /// </summary>
    /// <param name="bitArray"></param>
    /// <param name="upperPercentageLimit"></param>
    /// <returns></returns>
    public static bool IsSparse(this System.Collections.BitArray bitArray, int upperPercentageLimit)
        => bitArray.Count / 100 * bitArray.Population() < upperPercentageLimit;

}
