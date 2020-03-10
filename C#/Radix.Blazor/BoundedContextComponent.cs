using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class BoundedContextComponent<TViewModel, TCommand, TEvent> : Component<TViewModel, TCommand, TEvent> 
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new() where TEvent : Event
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="readModel"></param>
        /// <param name="context">The bounded context that is needed to send command</param>
        protected BoundedContextComponent(ReadModel<TViewModel, TEvent> readModel, BoundedContext<TCommand, TEvent> context) : base(readModel, context)
        {
            _context = context;
        }

        protected BoundedContext<TCommand, TEvent> _context;
    }
}
