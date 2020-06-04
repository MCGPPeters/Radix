using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Tests.Models;
using SqlStreamStore;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            IStreamStore streamStore = new InMemoryStreamStore();
            SqlStreamStore sqlStreamStore = new SqlStreamStore(streamStore);

            CheckForConflict<InventoryItemCommand, InventoryItemEvent, Json> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json> boundedContextSettings =
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    sqlStreamStore.AppendEvents,
                    sqlStreamStore.GetEventsSince,
                    checkForConflict,
                    new GarbageCollectionSettings(),
                    (parseEvent, parseMetaData, descriptor) =>
                    {
                        if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
                        {
                            return JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value);
                        }

                        if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
                        {
                            return JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value);
                        }

                        if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                        {
                            return JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
                        }

                        if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                        {
                            return JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
                        }

                        throw new InvalidOperationException("Unknown event");
                    },
                    (messageId, @event, serialize, eventMetaData, serializeMetaData) => new TransientEventDescriptor<Json>(
                        new EventType(@event.GetType().Name),
                        serialize(@event),
                        serializeMetaData(eventMetaData),
                        messageId),
                    input => new Json(JsonSerializer.Serialize(input)),
                    Json => new Json(JsonSerializer.Serialize(Json)));
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> boundedContext =
                new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(boundedContextSettings);

            IndexViewModel indexViewModel = new IndexViewModel();
            AddInventoryItemViewModel addInventoryItemViewModel = new AddInventoryItemViewModel();


            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexViewModel);
            builder.Services.AddSingleton(addInventoryItemViewModel);

            builder.RootComponents.Add<App>("app");


            await builder.Build().RunAsync();
        }
    }
}
