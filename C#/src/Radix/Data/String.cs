using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data
{


    public record String(string Value) : Alias<string>(Value), Monoid<String, Addition<String>>
    {

        public Addition<String> Combine => new((s, s2) => s + s2);

        public String Identity => string.Empty;

        public String Empty() => string.Empty;

        public static implicit operator String(string s) => new(s);

        public static implicit operator string(String @string) => @string.Value;

        public String Binary(String x, String y) => x + y;
    }
}
