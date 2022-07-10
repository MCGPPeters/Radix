﻿namespace Radix.Interaction.Data;

public record Attribute(string Name);

public record Attribute<T>(NodeId NodeId, string Name, params T[] Values) : Attribute(Name);
