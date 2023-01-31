using System.Security.Cryptography;
using System.Text;

namespace Radix.Data;

[Alias<System.Guid>]
public partial class DeterministicGuid : ParsableRead<DeterministicGuid>, IParsable<DeterministicGuid>
{
    public static DeterministicGuid Parse(string s, IFormatProvider? formatProviderprovider)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(s));
        }

        return CreateHashGuid(s);
    }

    private static DeterministicGuid CreateHashGuid(string s)
    {
        var provider = MD5.Create();
        var inputBytes = Encoding.Default.GetBytes(s);
        var hashedBytes = provider.ComputeHash(inputBytes);
        var hashGuid = (DeterministicGuid)new System.Guid(hashedBytes);

        return hashGuid;
    }


    public static bool TryParse(string? s, IFormatProvider? provider, out DeterministicGuid result)
    {
        result = (DeterministicGuid)System.Guid.Empty;
        if (s is null)
        {
            return false;
        }

        result = CreateHashGuid(s);
        return true;
    }
}
