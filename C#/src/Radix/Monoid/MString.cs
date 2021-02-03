using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data
{


    public record MString(string Value) : Alias<string>(Value), Monoid<MString, Addition<MString>>
    {
        public MString Empty() => string.Empty;

        public static implicit operator MString(string s) => new(s);

        public static implicit operator string(MString mString) => mString.Value;

        public MString Binary(MString x, MString y) => x + y;

        public Addition<MString> Combine
        {
            get => new((s, s2) => s + s2);
        }

        public MString Identity
        {
            get => string.Empty;
        }
    }
}
