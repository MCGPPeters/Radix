using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Shop.Data;
using Radix.Shop.Pages;
using Radix.Shop.Sales;
using Radix.Shop.Shared;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<NavMenuViewModel>();
builder.Services.AddSingleton(_ = new IndexViewModel((x, y) =>
{
    async IAsyncEnumerable<Product> GetProducts()
    { yield return new Product(Guid.NewGuid(), "foo", "bar", "what?", 20); };
    return GetProducts();
}));

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

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
