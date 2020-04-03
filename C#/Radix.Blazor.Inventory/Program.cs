using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Blazor.Inventory.Pages;
using Radix.Tests.Models;
using System.Linq;
using System.Threading.Tasks;
using static Radix.Option.Extensions;

namespace Radix.Blazor.Inventory
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
            builder.Services.AddBaseAddressHttpClient();

            builder.RootComponents.Add<App>("app");

            

            await builder.Build().RunAsync();
        }
    }
}
