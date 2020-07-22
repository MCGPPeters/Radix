using System;

namespace Radix
{
    public record MessageId(Guid Value) : Alias<Guid>(Value);

}
