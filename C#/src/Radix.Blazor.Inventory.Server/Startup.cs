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
using Radix.Option;
using Radix.Tests.Models;
using SqlStreamStore;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            IStreamStore streamStore = new InMemoryStreamStore();
            SqlStreamStore sqlStreamStore = new SqlStreamStore(streamStore);
            FromEventDescriptor<InventoryItemEvent, Json> fromEventDescriptor = (parseEvent, parseMetaData, descriptor) =>
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
        

            IndexViewModel indexViewModel = new IndexViewModel();
            streamStore.SubscribeToAll(0, 
                async (subscription, message, token) =>
                {
                    string jsonData = await message.GetJsonData();
                    string streamMessageType = message.Type;
                    var descriptor = new EventDescriptor<Json>(
                        new Json(jsonData),
                        message.StreamVersion,
                        new EventType(streamMessageType));

                    var @event = fromEventDescriptor(null, null, descriptor);
                    switch (@event)
                    {
                        case Some<InventoryItemCreated> (var inventoryItemCreated):
                            indexViewModel.InventoryItems.Add((inventoryItemCreated.Address, inventoryItemCreated.Name));
                            break;
                        case InventoryItemDeactivated _:
                            break;
                        case ItemsCheckedInToInventory _:
                            break;
                        case Some<ItemsRemovedFromInventory> (var inventoryItemsRemovedFromInventory):
                            var item = indexViewModel.InventoryItems.Find(tuple => tuple.address == inventoryItemsRemovedFromInventory.Address);
                            indexViewModel.InventoryItems.Remove(item);
                            break;
                        case Some<InventoryItemRenamed> (var inventoryItemRenamed):
                            var itemToRename = indexViewModel.InventoryItems.Find(tuple => tuple.address == inventoryItemRenamed.Address);
                            itemToRename.name = inventoryItemRenamed.Name;
                            break;
                        // ignore others like the metadatastream
                    }
                });

            CheckForConflict<InventoryItemCommand, InventoryItemEvent, Json> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            
            ToTransientEventDescriptor<InventoryItemEvent, Json> toTransientEventDescriptor = (messageId, @event, serialize, eventMetaData, serializeMetaData) =>
                new TransientEventDescriptor<Json>(new EventType(@event.GetType().Name), serialize(@event), serializeMetaData(eventMetaData), messageId);
            Serialize<InventoryItemEvent, Json> serializeEvent = input =>
            {
                JsonSerializerOptions options = new JsonSerializerOptions { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryItemEvent>() } };
                string jsonMessage = JsonSerializer.Serialize(input, options);
                return new Json(jsonMessage);
            };
            Serialize<EventMetaData, Json> serializeMetaData = json => new Json(JsonSerializer.Serialize(json));
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json> boundedContextSettings =
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    sqlStreamStore.AppendEvents,
                    sqlStreamStore.GetEventsSince,
                    checkForConflict,
                    new GarbageCollectionSettings(),
                    fromEventDescriptor,
                    toTransientEventDescriptor,
                    serializeEvent,
                    serializeMetaData);
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> boundedContext =
                new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(boundedContextSettings);


            Serialize<CounterIncremented, Json> serializeCounterEvent = input => new Json(JsonSerializer.Serialize(input));
            BoundedContextSettings<IncrementCommand, CounterIncremented, Json> counterBoundedContextSettings =
                new BoundedContextSettings<IncrementCommand, CounterIncremented, Json>(
                    sqlStreamStore.AppendEvents,
                    sqlStreamStore.GetEventsSince,
                    (command, descriptor) => new None<Conflict<IncrementCommand, CounterIncremented>>(),
                    new GarbageCollectionSettings(),
                    (parse, data, descriptor) => Some(new CounterIncremented()),
                    (id, @event, serialize, data, serializeEventMetaData) => new TransientEventDescriptor<Json>(
                        new EventType(@event.GetType().Name),
                        serializeCounterEvent(@event),
                        serializeEventMetaData(data),
                        id),
                    serializeCounterEvent,
                    serializeMetaData);

            BoundedContext<IncrementCommand, CounterIncremented, Json> counterBoundedContext =
                new BoundedContext<IncrementCommand, CounterIncremented, Json>(counterBoundedContextSettings);

            
            AddInventoryItemViewModel addInventoryItemViewModel = new AddInventoryItemViewModel();
            CounterViewModel counterViewModel = new CounterViewModel();

            services.AddSingleton(boundedContext);
            services.AddSingleton(counterBoundedContext);
            services.AddSingleton(indexViewModel);
            services.AddSingleton(addInventoryItemViewModel);
            services.AddSingleton(counterViewModel);

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
