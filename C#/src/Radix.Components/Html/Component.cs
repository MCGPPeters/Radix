﻿using System;
using System.Collections.Generic;

namespace Radix.Components.Html
{
    public delegate Component component(IAttribute[] attributes, params Node[] children);

    public delegate Component component<in T>(params T[] attributes) where T : IAttribute;

    public record Component(Type Type, IEnumerable<IAttribute> Attributes, IEnumerable<Node> Children) : Node;

}
