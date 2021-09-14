using System.Threading.Channels;
using Microsoft.Extensions.Caching.Memory;
using Radix.Result;
using Radix.Validated;

namespace Radix;

/// <summary>
///     Represent an instance of an aggregate root which can accept commands to process
/// </summary>
/// <typeparam name="TCommand">The type of command the aggregate can accept</typeparam>
/// <typeparam name="TEvent">The type of events the aggregate can produce</typeparam>
internal class AggregateInstance<TCommand, TEvent> : Aggregate<TCommand, TEvent> where TEvent : notnull
{
    private static readonly MemoryCache s_instances = new(new MemoryCacheOptions { });

    private readonly Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)> _channel;

    private Task _agent;
    private readonly CancellationTokenSource _tokenSource;


    /// <summary>
    ///     An aggregate instance should only be created by the runtime
    /// </summary>
    /// <param name="id"></param>
    /// <param name="accept"></param>
    public AggregateInstance(Id id, Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)> channel, Task agent, CancellationTokenSource tokenSource)
    {
        Id = id;
        _channel = channel;
        _agent = agent;
        _tokenSource = tokenSource;
    }

    /// <summary>
    ///     The id of the aggregate
    /// </summary>
    public Id Id { get; }

    /// <summary>
    ///     Accepts commands and returns either the resulting events or the errors that occurred
    /// </summary>
    public Accept<TCommand, TEvent> Accept
    {
        get => async validatedCommand =>
        {
            switch (validatedCommand)
            {
                case Valid<TCommand>(var validCommand):
                    TransientCommandDescriptor<TCommand> transientCommandDescriptor = new(Id, validCommand);
                    var taskCompletionSource = new TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>();
                    if (_channel is not null)
                    {
                        await _channel.Writer.WriteAsync((transientCommandDescriptor, taskCompletionSource)).ConfigureAwait(false);
                    }

                    return await taskCompletionSource.Task;

                case Invalid<TCommand>(var messages):
                    return Error<CommandResult<TEvent>, Error[]>(messages.Select(s => new Error(s)).ToArray());
                default: throw new InvalidOperationException();
            }
        };
    }
}
