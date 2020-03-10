using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            // IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> history = SqlStreamStore.GetEventsSince();
            // builder.Services.AddSingleton(await Initial<IndexViewModel, InventoryItemEvent>.State(history));

            builder.RootComponents.Add<InventoryItemBoundedContextComponent>("app");


            await builder.Build().RunAsync();
        }
    }


}
