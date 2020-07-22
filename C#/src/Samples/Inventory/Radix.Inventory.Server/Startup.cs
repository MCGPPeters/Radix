using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Blazor.Inventory.Server.Pages;
using Radix.Inventory.Domain;
using Radix.Option;
using Radix.Validated;
using SqlStreamStore;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public static List<(long id, string Name)> InventoryItems { get; set; } = new List<(long id, string Name)>();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            IStreamStore streamStore = new InMemoryStreamStore();
            SqlStreamStore sqlStreamStore = new SqlStreamStore(streamStore);
            FromEventDescriptor<InventoryItemEvent, Json> fromEventDescriptor = descriptor =>
            {
                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemCreated).FullName, StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemDeactivated).FullName, StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemRenamed).FullName, StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
                }

                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemRenamed).FullName, StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
                }

                return None<InventoryItemEvent>();
            };


            IndexViewModel indexViewModel = new IndexViewModel(InventoryItems);
            streamStore.SubscribeToAll(
                0,
                async (subscription, message, token) =>
                {
                    string jsonData = await message.GetJsonData();
                    string streamMessageType = message.Type;
                    EventType
                        .Create(streamMessageType)
                        .Map(eventType => new EventDescriptor<Json>(new Json(jsonData), message.StreamVersion, eventType))
                        .Map(eventDescriptor => fromEventDescriptor(eventDescriptor))
                        .Map(
                            optionalInventoryItemEvent =>
                            {
                                switch (optionalInventoryItemEvent)
                                {
                                    case Some<InventoryItemCreated>(var inventoryItemCreated):
                                        indexViewModel.InventoryItems.Add((inventoryItemCreated.Id, inventoryItemCreated.Name));
                                        break;
                                    case InventoryItemDeactivated _:
                                        break;
                                    case ItemsCheckedInToInventory _:
                                        break;
                                    case Some<ItemsRemovedFromInventory>(var inventoryItemsRemovedFromInventory):
                                        (long id, string name) item =
                                            indexViewModel.InventoryItems.Find(tuple => Equals(tuple.id, inventoryItemsRemovedFromInventory.Id));
                                        indexViewModel.InventoryItems.Remove(item);
                                        break;
                                    case Some<InventoryItemRenamed>(var inventoryItemRenamed):
                                        (long id, string name) itemToRename =
                                            indexViewModel.InventoryItems.Find(tuple => Equals(tuple.id, inventoryItemRenamed.Id));
                                        itemToRename.name = inventoryItemRenamed.Name;
                                        break;
                                    // ignore others like the metadatastream
                                }

                                return Unit.Instance;
                            });
                });

            Serialize<InventoryItemEvent, Json> serializeEvent = input =>
            {
                JsonSerializerOptions options = new JsonSerializerOptions {Converters = {new PolymorphicWriteOnlyJsonConverter<InventoryItemEvent>()}};
                string jsonMessage = JsonSerializer.Serialize(input, options);
                return new Json(jsonMessage);
            };
            Serialize<EventMetaData, Json> serializeMetaData = json => new Json(JsonSerializer.Serialize(json));

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
                },
                input => Some(JsonSerializer.Deserialize<EventMetaData>(input.Value)));
            BoundedContextSettings<InventoryItemEvent, Json> boundedContextSettings =
                new BoundedContextSettings<InventoryItemEvent, Json>(
                    sqlStreamStore.AppendEvents,
                    getEventsSince,
                    new GarbageCollectionSettings(),
                    fromEventDescriptor,
                    serializeEvent,
                    serializeMetaData);
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> boundedContext =
                new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(boundedContextSettings);


            GetEventsSince<CounterIncremented> getCounterIncrementsEventsSince = sqlStreamStore.CreateGetEventsSince(
                (json, type) =>
                {
                    if (string.Equals(type.Value, nameof(CounterIncremented), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<CounterIncremented>(json.Value));
                    }


                    return None<CounterIncremented>();
                },
                input => Some(JsonSerializer.Deserialize<EventMetaData>(input.Value)));
            Serialize<CounterIncremented, Json> serializeCounterEvent = input => new Json(JsonSerializer.Serialize(input));
            BoundedContextSettings<CounterIncremented, Json> counterBoundedContextSettings =
                new BoundedContextSettings<CounterIncremented, Json>(
                    sqlStreamStore.AppendEvents,
                    getCounterIncrementsEventsSince,
                    new GarbageCollectionSettings(),
                    descriptor => Some(new CounterIncremented()),
                    serializeCounterEvent,
                    serializeMetaData);

            BoundedContext<IncrementCommand, CounterIncremented, Json> counterBoundedContext =
                new BoundedContext<IncrementCommand, CounterIncremented, Json>(counterBoundedContextSettings);


            AddInventoryItemViewModel addInventoryItemViewModel = new AddInventoryItemViewModel();
            CounterViewModel counterViewModel = new CounterViewModel();
            DeactivateInventoryItemViewModel deactivateInventoryItemViewModel = new DeactivateInventoryItemViewModel(InventoryItems);

            services.AddSingleton(boundedContext);
            services.AddSingleton(counterBoundedContext);
            services.AddSingleton(indexViewModel);
            services.AddSingleton(addInventoryItemViewModel);
            services.AddSingleton(counterViewModel);
            services.AddSingleton(deactivateInventoryItemViewModel);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");
                });
        }
    }
}
