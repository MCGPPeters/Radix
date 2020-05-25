using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Tests.Models;
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

            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent> boundedContextSettings = new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                SqlStreamStore<InventoryItemEvent>.AppendEvents, SqlStreamStore<InventoryItemEvent>.GetEventsSince,
                checkForConflict,
                new GarbageCollectionSettings());
            BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(boundedContextSettings);

            IndexViewModel indexViewModel = State.Create(AsyncEnumerable.Empty<InventoryItemEvent>(), IndexViewModel.Update).Result;
            AddInventoryItemViewModel addInventoryItemViewModel = State.Create(AsyncEnumerable.Empty<InventoryItemEvent>(), AddInventoryItemViewModel.Update).Result;

            services.AddSingleton(boundedContext);
            services.AddSingleton(indexViewModel);
            services.AddSingleton(addInventoryItemViewModel);

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
