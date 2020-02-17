using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Async;
using Radix.Tests.Models;
using static System.Int32;
using static System.Text.Json.JsonSerializer;
using static Radix.Result.Extensions;

namespace Radix.Blazor.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<InventoryItemBoundedContextComponent>("app");

            await builder.Build().RunAsync();
        }
    }

    public class InventoryItemBoundedContextComponent : BoundedContextComponent<InventoryItemCommand, InventoryItemEvent>
    {
        private readonly IEventStoreConnection eventStoreConnection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));

        public override SaveEvents<InventoryItemEvent> SaveEvents => async (address, version, events) =>
        {

            var eventData = events.Select(
                @event =>
                {
                    var typeName = @event.GetType().ToString();
                    var eventType = char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
                    var eventAsJSON = SerializeToUtf8Bytes(@event);
                    return new EventData(@event.Id, eventType, true, eventAsJSON, Array.Empty<byte>());
                }).ToArray();

            Func<Task<WriteResult>> appendToStream = () => eventStoreConnection.AppendToStreamAsync(address.ToString(), version.Value, eventData);

            var result = await appendToStream.Retry(Backoff.Exponentially());

            return Ok<Version, SaveEventsError>(result.NextExpectedVersion);
        };

        public override GetEventsSince<InventoryItemEvent> GetEventsSince => async (address, version) =>
        {
            Func<Task<StreamEventsSlice>> readAllEventsForwardAsync = () => eventStoreConnection.ReadStreamEventsForwardAsync(address.ToString(), version.Value, MaxValue, false);

            var result = await readAllEventsForwardAsync.Retry(Backoff.Exponentially());

            return result.Events.Select(
                resolvedEvent =>
                {
                    var inventoryItemEvent = Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                    return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.OriginalEventNumber);
                });
        };

        public override ResolveRemoteAddress ResolveRemoteAddress => address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));

        public override Forward<InventoryItemCommand> Forward => (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
        public override FindConflicts<InventoryItemCommand, InventoryItemEvent> FindConflicts => (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
  
            );
        }
    }


}
