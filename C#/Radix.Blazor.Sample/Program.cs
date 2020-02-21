﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Blazor.Hosting;
using Radix.Async;
using Radix.Blazor.Html;
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

            Func<Task<WriteResult>> appendToStream;

            switch (version)
            {
                case AnyVersion anyVersion:
                    appendToStream = () => eventStoreConnection.AppendToStreamAsync($"InventoryItem-{address.ToString()}", anyVersion.Value, eventData);
                    var result = await appendToStream.Retry(Backoff.Exponentially());
                    return Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                case Version v:
                    appendToStream = () => eventStoreConnection.AppendToStreamAsync($"InventoryItem-{address.ToString()}", v.Value, eventData);
                    result = await appendToStream.Retry(Backoff.Exponentially());
                    return Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public override GetEventsSince<InventoryItemEvent> GetEventsSince => async (address, version) =>
        {
            switch (version)
            {
                case AnyVersion anyVersion:
                    Func<Task<StreamEventsSlice>> readAllEventsForwardAsync = () => eventStoreConnection.ReadStreamEventsForwardAsync(address.ToString(), anyVersion.Value, MaxValue, false);
                    var result = await readAllEventsForwardAsync.Retry(Backoff.Exponentially());
                    return result.Events.Select(
                        resolvedEvent =>
                        {
                            var inventoryItemEvent = Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                            return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.OriginalEventNumber);
                        });
                case Version v:
                    readAllEventsForwardAsync = () => eventStoreConnection.ReadStreamEventsForwardAsync(address.ToString(), v.Value, MaxValue, false);
                    result = await readAllEventsForwardAsync.Retry(Backoff.Exponentially());
                    return result.Events.Select(
                        resolvedEvent =>
                        {
                            var inventoryItemEvent = Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                            return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.OriginalEventNumber);
                        });
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public override ResolveRemoteAddress ResolveRemoteAddress => address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));

        public override Forward<InventoryItemCommand> Forward => (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
        public override FindConflicts<InventoryItemCommand, InventoryItemEvent> FindConflicts => (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

        public override OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected => (conflicts) => Task.FromResult(Unit.Instance);

        public override GarbageCollectionSettings GarbageCollectionSettings =>
            new GarbageCollectionSettings
            {
                ScanInterval = TimeSpan.FromMinutes(1),
                IdleTimeout = TimeSpan.FromMinutes(60)
            };


        protected override Node Render()
        {
            throw new NotImplementedException();
        }
    }


}
