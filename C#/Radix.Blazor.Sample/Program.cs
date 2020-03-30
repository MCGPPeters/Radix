using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radix.Blazor.Sample.Components;
using Radix.Tests.Models;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            var boundedContextSettings = new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                new SqlStreamStore<InventoryItemEvent>(),
                checkForConflict,
                new GarbageCollectionSettings());
            var boundedContext = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(boundedContextSettings);

            var indexReadModel = await ReadModel<IndexViewModel, InventoryItemEvent>.Create(AsyncEnumerable.Empty<InventoryItemEvent>());
            var addInventoryItemReadModel = await ReadModel<AddInventoryItemViewModel, InventoryItemEvent>.Create(AsyncEnumerable.Empty<InventoryItemEvent>());

            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexReadModel);
            builder.Services.AddSingleton(addInventoryItemReadModel);
            builder.Services.AddSingleton(s =>
            {
                IJSRuntime jsRuntime = s.GetRequiredService<IJSRuntime>();
                return new Dictionary<string, View>()
                {
                    { "Home", new IndexComponent(boundedContext, indexReadModel, jsRuntime) },
                    { "Add", new AddInventoryItemComponent(boundedContext, addInventoryItemReadModel, jsRuntime) },
                };
            });

            builder.RootComponents.Add<RouterComponent<InventoryItemCommand, InventoryItemEvent>>("#app");

            

            await builder.Build().RunAsync();
        }
    }
}
