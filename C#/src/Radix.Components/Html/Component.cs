﻿using Microsoft.AspNetCore.Components;

namespace Radix.Components.Html;

public record Component : Node
{
    public Component(Type type, IEnumerable<IAttribute> attributes, IEnumerable<Node> children)
    {
        Type = type;
        Attributes = attributes;
        Children = children;
    }

    public Component(Type type, IEnumerable<IAttribute> attributes)
    {
        Type = type;
        Attributes = attributes;
    }

    public Type Type { get; }
    public IEnumerable<IAttribute> Attributes { get; }
    public IEnumerable<Node> Children { get; } = new List<Node>();
}

public delegate Component component(IAttribute[] attributes, params Node[] children);

public delegate Component component<in T>(params T[] attributes) where T : IAttribute;
