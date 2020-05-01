using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Tests.Models;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent> boundedContextSettings = new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                SqlStreamStore<InventoryItemEvent>.AppendEvents, SqlStreamStore<InventoryItemEvent>.GetEventsSince,
                checkForConflict,
                new GarbageCollectionSettings());
            BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(boundedContextSettings);

            IndexViewModel indexViewModel = await State.Create(AsyncEnumerable.Empty<InventoryItemEvent>(), IndexViewModel.Update);
            AddInventoryItemViewModel addInventoryItemViewModel = await State.Create(AsyncEnumerable.Empty<InventoryItemEvent>(), AddInventoryItemViewModel.Update);


            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexViewModel);
            builder.Services.AddSingleton(addInventoryItemViewModel);

            builder.RootComponents.Add<App>("app");


            await builder.Build().RunAsync();
        }
    }
}
