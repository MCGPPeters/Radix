using static System.Net.Mime.MediaTypeNames;

namespace Radix.Data;

public record Reason(string Title, params string[] Descriptions)
{
    /// <summary>
    /// Pretty print the reason
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string output = "";
        output +=
        $"""
        {Title}:{Environment.NewLine}
        """;
        var descriptions = Descriptions.AsSpan();
        foreach (var description in descriptions)
        {
            output +=
            $"""
                    {description}{Environment.NewLine}
            """;
        }
        return output;
        
    }
};
