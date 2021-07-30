using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Radix
{
    public record Agent<TCommand, TEvent>
    {
        public DateTimeOffset LastActivity { get; set; }
        public Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)>? Channel { get; init; }
        public CancellationTokenSource? TokenSource { get; init; }
        public Task? Task { get; internal set; }
        public Accept<TCommand, TEvent>? Accept { get; init; }
    }




}
