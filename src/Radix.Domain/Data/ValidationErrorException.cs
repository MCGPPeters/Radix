using Radix.Data;

namespace Radix.Domain.Data;

public class ValidationErrorException : Exception
{
    public Reason[] Reasons { get; }

    public ValidationErrorException(params Reason[] reasons)
    {
        Reasons = reasons;
    }
}
