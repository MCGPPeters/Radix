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
using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;
using Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons;
using Radix.Components.Material._3._2._0.AppBar.Top;

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
    
       
        var regularAppBarModel = new RegularModel
        {
            ActionButtons = new()
        {
            new ShoppingCart(),
            new Search()
        },
            NavigationButton = new Menu(),
            PageTitle = "Foo!",
        };
        services.AddSingleton(_ => regularAppBarModel);
        services.AddSingleton(_ => new MenuButtonModel());
        services.AddSingleton(_ => new ShoppingCartButtonModel());
        services.AddSingleton(_ => new SearchButtonModel());

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddSingleton(indexViewModel);
        services.AddSingleton(_ => new NavMenuModel());
        services.AddTransient(_ => new AddItemModel());
        services.AddTransient(_ => indexViewModel);
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
