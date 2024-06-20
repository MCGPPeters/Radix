namespace Radix.Generators.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class LiteralAttribute : Attribute
{
    public string? StringRepresentation { get; set; }
}
