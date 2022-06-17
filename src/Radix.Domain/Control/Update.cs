namespace Radix.Domain.Control;

/// <summary>
/// Update takes the current state and some list of events and produces a new state
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <param name="state"></param>
/// <param name="event"></param>
/// <returns></returns>
public delegate TState Update<TState, in TEvent>(TState state, TEvent[] @event);

/// <summary>
/// The view produces some markup and an optional list of future messages, based on some state
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TMarkup"></typeparam>
/// <param name="model"></param>
/// <returns></returns>
public delegate (TMarkup Markup, IAsyncEnumerable<TCommand> Messages) View<TState, TCommand, TMarkup>(TState model);
