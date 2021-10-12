using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Shop.Catalog.Interface.Logic.Components;
using Radix.Shop.Data;
using Radix.Shop.Pages;
using Radix.Shop.Shared;
using Radix.Shop.Catalog.Domain;
using static Radix.Nullable.Extensions;
using static Radix.Option.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var httpClient = new HttpClient();

var configureAHApiEndPoint = AH.ConfigureSetHttpClient(httpClient);
Search<Product> searchAH = configureAHApiEndPoint(new Uri(builder.Configuration["CATALOG.SEARCH.AH.URI"]));
var searchViewModel = new SearchViewModel() { Search = Workflows.SearchAll(searchAH) };

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<NavMenuViewModel>();
builder.Services.AddSingleton<IndexViewModel>();
builder.Services.AddSingleton(searchViewModel);

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
