using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Inventory.Domain;
using Radix.Inventory.Shared;
using Radix.Data;
using Radix.Inventory.Pages;
using SqlStreamStore;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;

namespace Radix.Inventory;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public static List<ItemModel> InventoryItems { get; set; }
        = new();

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

        IndexModel indexViewModel = new IndexModel { InventoryItems = InventoryItems };
        Context context = new();

        IAllStreamSubscription? _ = Context.StreamStore.SubscribeToAll(
            0,
            async (subscription, message, token) =>
            {
                Radix.Domain.Data.Aggregate.Id? aggregateId = EventStreamDescriptor.FromString(message.StreamId).AggregateId;
                string jsonData = await message.GetJsonData();
                string streamMessageType = message.Type;
                EventType
                    .Create(streamMessageType)
                    .Map(eventType => new EventDescriptor<Json>(new Json(jsonData), message.StreamVersion, eventType))
                    .Map(eventDescriptor => context.FromEventDescriptor(eventDescriptor))
                    .Map(
                        optionalInventoryItemEvent =>
                        {
                            switch (optionalInventoryItemEvent)
                            {
                                case Some<ItemCreated>(var inventoryItemCreated):
                                    indexViewModel.InventoryItems.Add(new ItemModel { Id = aggregateId, Name = inventoryItemCreated.Name, Activated = inventoryItemCreated.Activated });
                                    break;
                                case Some<ItemDeactivated>(_):
                                    indexViewModel.InventoryItems =
                                        indexViewModel.InventoryItems
                                            .Select(x =>
                                            {
                                                if (aggregateId == x.Id)
                                                    x.Activated = false;
                                                return x;
                                            }
                                            ).ToList();
                                    break;
                                case Some<ItemsCheckedInToInventory> _:
                                    break;
                                case Some<ItemsRemovedFromInventory>(var inventoryItemsRemovedFromInventory):
                                    ItemModel item =
                                        indexViewModel.InventoryItems.Find(tuple => Equals(tuple.Id, inventoryItemsRemovedFromInventory.Id));
                                    indexViewModel.InventoryItems.Remove(item);
                                    break;
                                case Some<ItemRenamed>(var inventoryItemRenamed):
                                    indexViewModel.InventoryItems =
                                        indexViewModel.InventoryItems
                                            .Select(item =>
                                            {
                                                if (aggregateId == item.Id)
                                                    item.Name = inventoryItemRenamed.Name;
                                                return item;
                                            }).ToList();
                                    break;
                                    // ignore others like the metadatastream
                            }

                            return new Unit();
                        });
            });

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton<Context<IncrementCommand, CounterIncremented, Json>>(_ => new CounterContext());
        services.AddSingleton<Context<ItemCommand, ItemEvent, Json>>(_ => context);
        services.AddSingleton(indexViewModel);
        services.AddSingleton(_ => new NavMenuModel());
        services.AddTransient(_ => new AddItemModel());
        services.AddSingleton(new CounterModel());
        services.AddTransient(_ => new DeactivateInventoryItemModel());

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
