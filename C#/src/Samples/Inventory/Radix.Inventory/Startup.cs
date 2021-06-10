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

        public static List<(long id, string name, bool activated)> InventoryItems { get; set; }
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


            IndexViewModel indexViewModel = new(InventoryItems);
            var _ = streamStore.SubscribeToAll(
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
                                        indexViewModel.InventoryItems.Add((inventoryItemCreated.Id, inventoryItemCreated.Name, inventoryItemCreated.Activated));
                                        break;
                                    case Some<InventoryItemDeactivated>(var inventoryItemDeactivated):
                                        indexViewModel.InventoryItems =
                                            indexViewModel.InventoryItems
                                                .Select(item => inventoryItemDeactivated.Id == item.id ? (item.id, item.name, false) : item).ToList();
                                        break;
                                    case Some<ItemsCheckedInToInventory> _:
                                        break;
                                    case Some<ItemsRemovedFromInventory>(var inventoryItemsRemovedFromInventory):
                                        (long id, string name, bool activated) item =
                                            indexViewModel.InventoryItems.Find(tuple => Equals(tuple.id, inventoryItemsRemovedFromInventory.Id));
                                        indexViewModel.InventoryItems.Remove(item);
                                        break;
                                    case Some<InventoryItemRenamed>(var inventoryItemRenamed):
                                        indexViewModel.InventoryItems =
                                            indexViewModel.InventoryItems
                                                .Select(item => inventoryItemRenamed.Id == item.id ? (item.id, inventoryItemRenamed.Name, item.activated) : item).ToList();
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
                new(
                    sqlStreamStore.AppendEvents,
                    getEventsSince,
                    new GarbageCollectionSettings(),
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
                        return Some(JsonSerializer.Deserialize<CounterIncremented>(json.Value));
                    }


                    return None<CounterIncremented>();
                },
                input => Some(JsonSerializer.Deserialize<EventMetaData>(input.Value)));
            Serialize<CounterIncremented, Json> serializeCounterEvent = input => new Json(JsonSerializer.Serialize(input));
            BoundedContextSettings<CounterIncremented, Json> counterBoundedContextSettings =
                new(
                    sqlStreamStore.AppendEvents,
                    getCounterIncrementsEventsSince,
                    new GarbageCollectionSettings(),
                    descriptor => Some(new CounterIncremented()),
                    serializeCounterEvent,
                    serializeMetaData);

            BoundedContext<IncrementCommand, CounterIncremented, Json> counterBoundedContext =
                new(counterBoundedContextSettings);


            AddInventoryItemViewModel addInventoryItemViewModel = new();
            CounterViewModel counterViewModel = new();
            DeactivateInventoryItemViewModel deactivateInventoryItemViewModel = new(InventoryItems);

            services.AddSingleton(boundedContext);
            services.AddSingleton(counterBoundedContext);
            services.AddSingleton(indexViewModel);
            services.AddTransient(_ => new AddInventoryItemViewModel());
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