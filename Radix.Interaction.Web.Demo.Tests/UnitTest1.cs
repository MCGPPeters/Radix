using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radix.Interaction.Web.Demo.Pages;

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

    [Fact(DisplayName = "Given the count is 9, when incrementing, the count should be decreased by 1")]
    public async Task Test3()
    {
        var index = new Pages.Index();

        var view = index.Render();

        var tree = new RenderTreeBuilder();

        //tree.GetFrames().Array[0].;

        //Assert.Equal("", output.ToHtmlString());
    }
}

