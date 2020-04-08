using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Blazor.Inventory.Pages;
using Radix.Tests.Models;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            BoundedContextSettings<InventoryItemCommand, InventoryItemEvent> boundedContextSettings = new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                new SqlStreamStore<InventoryItemEvent>(),
                checkForConflict,
                new GarbageCollectionSettings());
            BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(boundedContextSettings);

            ReadModel<IndexViewModel, InventoryItemEvent> indexReadModel = await ReadModel<IndexViewModel, InventoryItemEvent>.Create(AsyncEnumerable.Empty<InventoryItemEvent>());
            ReadModel<AddInventoryItemViewModel, InventoryItemEvent> addInventoryItemReadModel =
                await ReadModel<AddInventoryItemViewModel, InventoryItemEvent>.Create(AsyncEnumerable.Empty<InventoryItemEvent>());


            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexReadModel);
            builder.Services.AddSingleton(addInventoryItemReadModel);
            builder.Services.AddBaseAddressHttpClient();

            builder.RootComponents.Add<App>("app");


            await builder.Build().RunAsync();
        }
    }
}
