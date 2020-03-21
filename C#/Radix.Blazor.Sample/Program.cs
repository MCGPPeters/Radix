using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
            builder.Services.AddBaseAddressHttpClient();

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

            builder.RootComponents.Add<AddInventoryItemComponent>("#app");


            var host = builder.Build();

            await host.RunAsync();
        }
    }


}
