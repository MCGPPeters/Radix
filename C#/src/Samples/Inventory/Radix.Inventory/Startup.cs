using System.Text.Json;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Blazor.Inventory.Server.Pages;
using Radix.Inventory.Domain;
using Radix.Option;
using Radix.Validated;
using SqlStreamStore;

namespace Radix.Inventory;

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

        
        IndexViewModel indexViewModel = new(InventoryItems);
        InventoryBoundedContext inventoryBoundedContext = new InventoryBoundedContext();

        IAllStreamSubscription? _ = InventoryBoundedContext.StreamStore.SubscribeToAll(
            0,
            async (subscription, message, token) =>
            {
                Id? aggregateId = EventStreamDescriptor.FromString(message.StreamId).AggregateId;
                string jsonData = await message.GetJsonData();
                string streamMessageType = message.Type;
                EventType
                    .Create(streamMessageType)
                    .Map(eventType => new EventDescriptor<Json>(new Json(jsonData), message.StreamVersion, eventType))
                    .Map(eventDescriptor => inventoryBoundedContext.FromEventDescriptor(eventDescriptor))
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

        AddInventoryItemViewModel addInventoryItemViewModel = new();
        CounterViewModel counterViewModel = new();
        DeactivateInventoryItemViewModel deactivateInventoryItemViewModel = new();

        services.AddSingleton< BoundedContext<IncrementCommand, CounterIncremented, Json>>(_ => new CounterBoundedContext());
        services.AddSingleton< BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>>(_ => inventoryBoundedContext);
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
