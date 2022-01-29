using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Components;

public delegate int Render(object currentComponent, RenderTreeBuilder builder, int sequence, Node node);
