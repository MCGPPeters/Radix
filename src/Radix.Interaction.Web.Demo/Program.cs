using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Interaction.Web.Demo;
using Radix.Interaction.Web.Demo.Pages;
using CounterModel = Radix.Interaction.Web.Demo.Pages.CounterModel;
using IndexModel = Radix.Interaction.Web.Demo.Pages.IndexModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddServerComponents().AddWebAssemblyComponents();
builder.Services.AddSingleton<IndexModel>();
builder.Services.AddSingleton<WeatherModel>();
builder.Services.AddSingleton<CounterModel>();
builder.Services.AddSingleton<InventoryModel>();
builder.Services.AddSingleton<AddItemModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddWebAssemblyRenderMode().AddServerRenderMode();

app.Run();
