namespace Radix.Gymnasium;

public static partial class Environment
{
    public static class RenderModes
    {
        public static RenderMode Human => new Human();
        public static RenderMode RgbArray => new RgbArray();
        public static RenderMode Ansi => new Ansi();
    }
    [Literal(StringRepresentation = "human")]
    public record Human : RenderMode;

    [Literal(StringRepresentation = "rgb_array")]
    public record RgbArray : RenderMode;

    [Literal(StringRepresentation = "ansi")]
    public record Ansi : RenderMode;
}
