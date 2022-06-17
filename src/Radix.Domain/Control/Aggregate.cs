using Radix.Data;
using Radix.Domain.Data;

namespace Radix.Domain.Control;

/// <summary>
///     The aggregate delegate represents the type of a function accepts a commands and produced events
/// </summary>
/// <typeparam name="TCommand">The root type of the command in the hierarchy of commands for the aggregate</typeparam>
/// <typeparam name="TEvent">The root type of the event in the hierarchy of events for the aggregate</typeparam>
/// <param name="validatedCommand"></param>
/// <returns>Either a list of events when the command processing succeeds or list of errors when it does not</returns>
public delegate Task<Result<CommandResult<TEvent>, Error[]>> Aggregate<TCommand, TEvent>(Validated<TCommand> validatedCommand);


