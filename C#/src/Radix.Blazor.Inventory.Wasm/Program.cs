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

            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            FromEventDescriptor<InventoryItemEvent, Json> fromEventDescriptor = descriptor =>
            {
                if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
                }

                return None<InventoryItemEvent>();
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = sqlStreamStore.CreateGetEventsSince<InventoryItemEvent>(
                (json, type) =>
                {
                    if (string.Equals(type.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<InventoryItemCreated>(json.Value));
                    }

                    if (string.Equals(type.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<InventoryItemDeactivated>(json.Value));
                    }

                    if (string.Equals(type.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(json.Value));
                    }

                    if (string.Equals(type.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(json.Value));
                    }

                    return None<InventoryItemEvent>();
                }, input => Some(JsonSerializer.Deserialize<EventMetaData>(input.Value)));

            ToTransientEventDescriptor<InventoryItemEvent, Json> toTransientEventDescriptor = (messageId, @event, serialize, eventMetaData, serializeMetaData) =>
                new TransientEventDescriptor<Json>(new EventType(@event.GetType()), serialize(@event), serializeMetaData(eventMetaData), messageId);
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json> boundedContextSettings =
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    sqlStreamStore.AppendEvents,getEventsSince,
                    checkForConflict,
                    new GarbageCollectionSettings(), fromEventDescriptor, toTransientEventDescriptor, input => new Json(JsonSerializer.Serialize(input)),
                    input => new Json(JsonSerializer.Serialize(input)));
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
