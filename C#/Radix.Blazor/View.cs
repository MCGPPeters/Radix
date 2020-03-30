using Radix.Blazor.Html;
using System;

namespace Radix.Blazor
{
    public interface View
    {
        Node Render();
    }
}