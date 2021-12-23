using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;
using Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons;
using Radix.Shop.Wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var regularAppBarViewModel = new Radix.Components.Material._3._2._0.AppBar.Top.RegularViewModel
{
    ActionButtons = new()
    {
        new ShoppingCart()
    },
    NavigationButton = new Menu(),
    PageTitle = "Radix"
};

builder.Services.AddSingleton<Radix.Components.Material._3._2._0.AppBar.Top.RegularViewModel>(_ => regularAppBarViewModel);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
});

await builder.Build().RunAsync();
