namespace Radix.Interaction.Data;

public record Attribute(string Name);

public record Attribute<T>(AttributeId AttributeId, string Name, params T[] Values) : Attribute(Name);
