using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Result;
using static Radix.Option.Extensions;

namespace Radix.Components;

public abstract class TaskBasedComponent<TViewModel, TCommand, TEvent, TFormat> : Component<TViewModel>
    where TViewModel : ViewModel
    where TEvent : notnull
{
    [Inject] public BoundedContext<TCommand, TEvent, TFormat> BoundedContext { get; set; } = null!;

    private bool _shouldRender;
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    


    /// <summary>
    /// Called when the Dispatch method resulted in events being generated. Usefull to override when not navigating
    /// to another component and the state presented in this component has to be updated
    /// </summary>
    protected virtual Update<TViewModel, TEvent> Update { get; } = (state, events) => state;

    /// <summary>
    /// Execute a validated command by sending the message to an aggregate. If the command is valid, the command will be processed
    /// by the aggregate. When the command has been handled successfully, and events are returned as a result, the events
    /// are aggregated to create the next state of the viewmodel, by calling the user defined Update function.
    /// Then a notification is sent to signal the state of the viewmodel has changed. This will trigger the component
    /// to rerender, so that it can show the correct state in the user interface. This rerendering triggers the user defined View
    /// function, which signifies how the view should be rendered
    /// </summary>
    /// <param name="target">The aggrgate that will handle the command</param>
    /// <param name="command">The validate command that the aggregate will handle, if it is valid. </param>
    /// <returns>When the command is not valid, it will return Some errors, None otherwise</returns>
    protected async Task<Option<Error[]>> Dispatch(Aggregate<TCommand, TEvent> target, Validated<TCommand> command)
    {
        Result<CommandResult<TEvent>, Error[]> result = await target.Accept(command);
        switch (result)
        {
            case Ok<CommandResult<TEvent>, Error[]>(var commandResult):
                _shouldRender = commandResult.Events.Any();
                ViewModel = Update(ViewModel, commandResult.Events);
                StateHasChanged();
                return None<Error[]>();
            case Error<CommandResult<TEvent>, Error[]>(var errors):
                _shouldRender = true;
                ViewModel.Errors = errors;
                StateHasChanged();
                return Some(errors);
            default:
                _shouldRender = false;
                throw new ArgumentOutOfRangeException(nameof(result));
        }
    }

    protected override bool ShouldRender() => _shouldRender;
}
