using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Blazor.Inventory.Server.Pages;
using Radix.Inventory.Domain;
using Radix.Option;
using Radix.Validated;
using SqlStreamStore;
using static Radix.Option.Extensions;

namespace Radix.Inventory
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public static List<InventoryItemModel> InventoryItems { get; set; }
            = new();

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            IStreamStore streamStore = new InMemoryStreamStore();
            SqlStreamStore sqlStreamStore = new(streamStore);
            FromEventDescriptor<InventoryItemEvent, Json> fromEventDescriptor = descriptor =>
            {
                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemCreated).FullName, StringComparison.Ordinal))
                {
                    InventoryItemCreated? inventoryItemCreated = JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value);
                    return inventoryItemCreated.AsOption();
                }

                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemDeactivated).FullName, StringComparison.Ordinal))
                {
                    InventoryItemDeactivated? inventoryItemDeactivated = JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value);
                    return inventoryItemDeactivated.AsOption();
                }

                if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemRenamed).FullName, StringComparison.Ordinal))
                {
                    InventoryItemRenamed? inventoryItemRenamed = JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
                    return inventoryItemRenamed.AsOption();
                }

                return None<InventoryItemEvent>();
            };


            IndexViewModel indexViewModel = new(InventoryItems);
            var _ = streamStore.SubscribeToAll(
                0,
                async (subscription, message, token) =>
                {
                    var aggregateId = EventStreamDescriptor.FromString(message.StreamId).AggregateId;
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

                                        indexViewModel.InventoryItems.Add(new InventoryItemModel(aggregateId, inventoryItemCreated.Name, inventoryItemCreated.Activated));
                                        break;
                                    case Some<InventoryItemDeactivated>(_):
                                        indexViewModel.InventoryItems =
                                            indexViewModel.InventoryItems
                                                .Select(x =>
                                                {
                                                    if (aggregateId == x.id)
                                                        x.activated = false;
                                                    return x;
                                                }
                                                ).ToList();
                                        break;
                                    case Some<ItemsCheckedInToInventory> _:
                                        break;
                                    case Some<ItemsRemovedFromInventory>(var inventoryItemsRemovedFromInventory):
                                        InventoryItemModel item =
                                            indexViewModel.InventoryItems.Find(tuple => Equals(tuple.id, inventoryItemsRemovedFromInventory.Id));
                                        indexViewModel.InventoryItems.Remove(item);
                                        break;
                                    case Some<InventoryItemRenamed>(var inventoryItemRenamed):
                                        indexViewModel.InventoryItems =
                                            indexViewModel.InventoryItems
                                                .Select(item =>
                                                {
                                                    if (aggregateId == item.id)
                                                        item.name = inventoryItemRenamed.Name;
                                                    return item;
                                                }).ToList();
                                        break;
                                        // ignore others like the metadatastream
                                }

                                return Unit.Instance;
                            });
                });

            Serialize<InventoryItemEvent, Json> serializeEvent = input =>
            {
                JsonSerializerOptions options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryItemEvent>() } };
                string jsonMessage = JsonSerializer.Serialize(input, options);
                return new Json(jsonMessage);
            };
            Serialize<EventMetaData, Json> serializeMetaData = json => new Json(JsonSerializer.Serialize(json));

            GetEventsSince<InventoryItemEvent> getEventsSince = sqlStreamStore.CreateGetEventsSince<InventoryItemEvent>(
                (json, type) =>
                {
                    if (string.Equals(type.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
                    {
                        InventoryItemCreated? inventoryItemCreated = JsonSerializer.Deserialize<InventoryItemCreated>(json.Value);
                        return inventoryItemCreated.AsOption();
                    }

                    if (string.Equals(type.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
                    {
                        InventoryItemDeactivated? inventoryItemDeactivated = JsonSerializer.Deserialize<InventoryItemDeactivated>(json.Value);
                        return inventoryItemDeactivated.AsOption();
                    }


                    if (string.Equals(type.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                    {
                        InventoryItemRenamed? inventoryItemRenamed = JsonSerializer.Deserialize<InventoryItemRenamed>(json.Value);
                        return inventoryItemRenamed.AsOption();
                    }

                    return None<InventoryItemEvent>();
                },
                input =>
                {
                    EventMetaData? eventMetaData = JsonSerializer.Deserialize<EventMetaData>(input.Value);
                    return eventMetaData.AsOption();
                });
            GarbageCollectionSettings garbageCollectionSettings = new();
            BoundedContextSettings<InventoryItemEvent, Json> boundedContextSettings =
                new(
                    sqlStreamStore.AppendEvents,
                    getEventsSince,
                    garbageCollectionSettings,
                    fromEventDescriptor,
                    serializeEvent,
                    serializeMetaData);
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> boundedContext =
                new(boundedContextSettings);


            GetEventsSince<CounterIncremented> getCounterIncrementsEventsSince = sqlStreamStore.CreateGetEventsSince(
                (json, type) =>
                {
                    if (string.Equals(type.Value, nameof(CounterIncremented), StringComparison.Ordinal))
                    {
                        CounterIncremented? counterIncremented = JsonSerializer.Deserialize<CounterIncremented>(json.Value);
                        return counterIncremented.AsOption();
                    }


                    return None<CounterIncremented>();
                },
                input =>
                {
                    EventMetaData? eventMetaData = JsonSerializer.Deserialize<EventMetaData>(input.Value);
                    return eventMetaData.AsOption();
                });
            Serialize<CounterIncremented, Json> serializeCounterEvent = input => new Json(JsonSerializer.Serialize(input));
            BoundedContextSettings<CounterIncremented, Json> counterBoundedContextSettings =
                new(
                    sqlStreamStore.AppendEvents,
                    getCounterIncrementsEventsSince,
                    garbageCollectionSettings,
                    descriptor => Some(new CounterIncremented()),
                    serializeCounterEvent,
                    serializeMetaData);

            BoundedContext<IncrementCommand, CounterIncremented, Json> counterBoundedContext =
                new(counterBoundedContextSettings);


            AddInventoryItemViewModel addInventoryItemViewModel = new();
            CounterViewModel counterViewModel = new();
            DeactivateInventoryItemViewModel deactivateInventoryItemViewModel = new();

            services.AddSingleton(boundedContext);
            services.AddSingleton(counterBoundedContext);
            services.AddSingleton(indexViewModel);
            services.AddTransient(_ => new AddInventoryItemViewModel());
            services.AddSingleton(counterViewModel);
            services.AddTransient(_ => new DeactivateInventoryItemViewModel());

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
