using Radix.Data;

namespace Radix;

public delegate Task<Result<TEvent[], CommandDecisionError>> Decide<in TState, in TCommand, TEvent>(TState state, TCommand command);
