namespace Radix;

public class Error
{
    public required string Message { get; set; }

    public override string ToString() => Message;

    public static implicit operator Error(string m) => new Error { Message = m};
}
