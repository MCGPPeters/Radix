﻿using System;
using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Blazor.Html
{
    public class ExplicitAttribute : IAttribute
    {
        public ExplicitAttribute(Name name, Func<RenderTreeBuilder, int, object, int> factory)
        {
            Name = name;
            Factory = factory;
        }

        public Name Name { get; set; }
        public Func<RenderTreeBuilder, int, object, int> Factory { get; }
    }
}
