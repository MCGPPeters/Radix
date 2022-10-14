using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck;
using FsCheck.Xunit;
using Radix.Data.Collections.Generic;

namespace Radix.Tests;
public class ListProperties
{
    [Property(
            DisplayName =
                "Enumerating the empty list should never yield any result")]
    public void Test1()
    {
        int i = 0;

        var empty = new EmptyList<int>();

        foreach (int item in empty) i++;

        Equals(0, i);
    }

    [Property(
            DisplayName =
                "Enumerating the non empty list should yield the correct results", Verbose = true)]
    public void Test2(NonEmptyArray<int> list)
    {
        int i = 0;

        var nonEmpty = Data.Collections.Generic.List<int>.Create(list.Get);

        foreach (int item in nonEmpty) i++;

        Xunit.Assert.Equal(list.Get.Length, i);
    }

    [Property(
            DisplayName =
                "The count of an empty list is zero", Verbose = true)]
    public void Test3()
    {

        var empty = Data.Collections.Generic.List<int>.Create(Enumerable.Empty<int>());


        Xunit.Assert.Equal(0, empty.Count);
    }

    [Property(
            DisplayName =
                "The count of an non empty list is correct", Verbose = true)]
    public void Test4(NonEmptyArray<int> list)
    {

        var nonEmpty = Data.Collections.Generic.List<int>.Create(list.Get);


        Xunit.Assert.Equal(list.Get.Length, nonEmpty.Count);
    }
}

