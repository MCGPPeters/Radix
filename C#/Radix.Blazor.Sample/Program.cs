using System.Linq;
using System.Threading.Tasks;
using Radix.Blazor.Sample.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

            FindConflict<InventoryItemCommand, InventoryItemEvent> findConflict = (command, descriptor) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = conflict => Task.FromResult(Unit.Instance);
            var boundedContextSettings = new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                new SqlStreamStore<InventoryItemEvent>(),
                findConflict,
                onConflictingCommandRejected,
                new GarbageCollectionSettings());
            var boundedContext = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(boundedContextSettings);

            var indexReadModel = await ReadModel<IndexViewModel, InventoryItemEvent>.Create(AsyncEnumerable.Empty<InventoryItemEvent>());

            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexReadModel);

            builder.RootComponents.Add<IndexComponent>("#app");


            var host = builder.Build();

            await host.RunAsync();
        }
    }


}
