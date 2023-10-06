using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radix.Interaction.Web.Demo.Pages;
using Radix.Interaction.Web.Demo.Shared;
using Counter = Radix.Interaction.Web.Demo.Shared.Counter;

namespace Radix.Interaction.Web.Demo.Tests;

public class CounterComponentTest
{
    [Fact(DisplayName = "Given the count is zero, when incrementing, the count should increase by 1")]
    public async Task Test1()
    {
        var counter = new Counter();

        var model = await counter.Update(new CounterModel(){Count = 0}, new Increment());

        Assert.Equal(1, model.Count);

    }

    [Fact(DisplayName = "Given the count is 9, when incrementing, the count should be decreased by 1")]
    public async Task Test2()
    {
        var counter = new Counter();

        var model = await counter.Update(new CounterModel() { Count = 9 }, new Decrement());

        Assert.Equal(8, model.Count);
    }

    [Fact(DisplayName = "The Index HTML should be correct")]
    public async Task Test3()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await htmlRenderer.RenderComponentAsync<Pages.Index>();

            return output.ToHtmlString();
        });

        Assert.Equal("<h1>Home</h1>Welcome to your new app.<div class=\"alert alert-secondary mt-4\"><span class=\"oi oi-pencil me-2\" aria-hidden=\"true\"></span><strong>How is Blazor working for you? </strong><span class=\"text-nowrap\">Please take our <a href=\"https://go.microsoft.com/fwlink/?linkid=2186158\" class=\"font-weight-bold link-dark\">brief survey</a> and tell us what you think.</span></div>", html);
    }
}

