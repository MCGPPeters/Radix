﻿using Radix.Interaction.Data;

namespace Radix.Interaction.Components;

public record ComponentAttribute(AttributeId Id, string Name, object Value) : Attribute<object>(Id, Name, Value);