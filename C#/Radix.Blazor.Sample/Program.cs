using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace Radix.Blazor.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<InventoryItemBoundedContextComponent>("app");


            await builder.Build().RunAsync();
        }
    }


}
