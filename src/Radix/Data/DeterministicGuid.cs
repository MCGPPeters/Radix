using System.Security.Cryptography;
using System.Text;
using Radix.Generators.Attributes;

namespace Radix.Data;

[Alias<System.Guid>]
public partial class DeterministicGuid
{
    /// <summary>
    /// Initializes a new instance of the DeterministicGuid struct.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DeterministicGuid Create(string s)
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
}
