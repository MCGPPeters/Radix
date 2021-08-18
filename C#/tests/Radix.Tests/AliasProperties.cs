using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck.Xunit;
using Xunit;

namespace Radix.Tests
{
    public class AliasProperties
    {
        //[Trait("Category", "Functor")]
        //[Property(DisplayName = "Must preserve identity morphisms")]
        //public void Test1(int i)
        //{
        //    Func<int, int> id = x => x;
        //    Alias<int> intAlias = Ok<int, int>(i);

        //    Equal(result, result.Map(id));
        //}

        //[Trait("Category", "Functor")]
        //[Property(DisplayName = "Preserve composition of morphisms")]
        //public void Test2(int i)
        //{
        //    Func<string, int> f = x => int.Parse(x);
        //    Func<int, string> g = x => x.ToString();
        //    Result<int, string> result = Ok<int, string>(i);

        //    Equal(result.Map(g).Map(f), result.Map(x => f(g(x))));
        //}
    }
}
