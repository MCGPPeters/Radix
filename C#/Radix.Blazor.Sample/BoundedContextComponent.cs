using Microsoft.AspNetCore.Components;

namespace Radix.Blazor.Sample
{
    public abstract class BoundedContextComponent<TCommand, TEvent> : ComponentBase
    {
        protected BoundedContext<TCommand, TEvent>? context;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            context = new BoundedContext<TCommand, TEvent>(new BoundedContextSettings<TCommand, TEvent>(SaveEvents, GetEventsSince, ResolveRemoteAddress, Forward, FindConflicts));
        }

        public abstract SaveEvents<TEvent> SaveEvents { get; }
        public abstract GetEventsSince<TEvent> GetEventsSince { get; }
        public abstract ResolveRemoteAddress ResolveRemoteAddress { get; }
        public abstract Forward<TCommand> Forward { get; }
        public abstract FindConflicts<TCommand, TEvent> FindConflicts { get; }
    }
}
