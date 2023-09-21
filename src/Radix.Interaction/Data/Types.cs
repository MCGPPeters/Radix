using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Interaction.Data;

public delegate void Node(object parentComponent, RenderTreeBuilder builder);

public delegate Node Attribute();

public delegate Node Element(Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0);

public delegate Node Component<in T>(Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0);

public delegate Attribute Event(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0);


[Alias<int>]
public partial struct NodeId { }
