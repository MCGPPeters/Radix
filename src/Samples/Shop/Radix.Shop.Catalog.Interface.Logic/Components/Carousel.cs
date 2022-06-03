using Microsoft.JSInterop;
using Radix.Components;

using Microsoft.AspNetCore.Components.Web.Extensions.Head;
using Radix.Interaction;
using Radix.Interaction.Data;
using Microsoft.AspNetCore.Components;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public class Carousel : Component<CarouselModel, CarouselCommand>, IAsyncDisposable
{
    private IJSObjectReference? _glider;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<CarouselModel, CarouselCommand> Update => (model, _) => Task.FromResult(model);

    protected override View<CarouselModel, CarouselCommand> View =>
        async (model, dispatch) =>
            concat
            (
                (NodeId)1,
                component<Link>
                (
                    (NodeId)2,

                    new[]
                    {
                        rel((AttributeId)1, "stylesheet"),
                        href((AttributeId)2, "https://unpkg.com/@glidejs/glide@3.5.0/dist/css/glide.core.min.css")
                    }
                ),
                script
                (
                    (NodeId)3,
                    text
                    (
                        (NodeId)31,
                        $"function createGlideInstance() {{ return new Glide('.{model.Id}').mount()}};"
                    )
                ),
                div
                (
                    (NodeId)4,
                    @class((AttributeId)3,model.Id),
                    concat
                    (
                        (NodeId)5,
                        div
                        (
                            (NodeId)6,
                            new[]
                            {
                                @class((AttributeId)4, "glide__track"),
                                attribute((AttributeId)5, "data-glide-el", "track")
                            },
                            ul
                            (
                                (NodeId)7,
                                new[] { @class((AttributeId)6, "glide__slides") },
                                model.Children.Select
                                (
                                    element =>
                                        li
                                        (
                                            (NodeId)8,
                                            new[] { @class((AttributeId)7, "glide__slide") },
                                            element
                                        )
                                ).ToArray()
                            )
                        ),
                        div
                        (
                            (NodeId)9,
                            new[]
                            {
                                @class((AttributeId)8, "glide__arrows"),
                                attribute((AttributeId)9, "data-glide-el", "controls")
                            },
                            button
                            (
                                (NodeId)10,
                                new[]
                                {
                                    @class((AttributeId)10, "slider__arrow slider__arrow--next glide__arrow glide__arrow--left"),
                                    attribute((AttributeId)11, "data-glide-dir", "<")
                                },
                                svg
                                (
                                    (NodeId)11,
                                    new[]
                                    {
                                        attribute((AttributeId)12, "xmlns", "http://www.w3.org/2000/svg"),
                                        width((AttributeId)13, "18"),
                                        height((AttributeId)14, "18"),
                                        attribute((AttributeId)15, "viewBox", "0 0 24 24")
                                    },
                                    element
                                    (
                                        (NodeId)12,
                                        "path",
                                        new[]
                                        {
                                            attribute((AttributeId)16, "d", "M0 12l10.975 11 2.848-2.828-6.176-6.176H24v-3.992H7.646l6.176-6.176L10.975 1 0 12z")
                                        }
                                    )
                                )
                            ),
                            button
                            (
                                (NodeId)13,
                                new[]
                                {
                                    @class((AttributeId)17, "slider__arrow slider__arrow--next", "glide__arrow", "glide__arrow--right"),
                                    attribute((AttributeId)18, "data-glide-dir", ">")
                                },
                                svg
                                (
                                    (NodeId)14,
                                    new[]
                                    {
                                        attribute((AttributeId)19, "xmlns", "http://www.w3.org/2000/svg"),
                                        width((AttributeId)20, "18"),
                                        height((AttributeId)21, "18"),
                                        attribute((AttributeId)22, "viewBox", "0 0 24 24")
                                    },
                                    element
                                    (
                                        (NodeId)15,
                                        "path",
                                        new[]
                                        {
                                            attribute((AttributeId)23, "d", "M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z")
                                        }
                                    )
                                )
                            )
                        )
                    )
                )
            );

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _glider = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://unpkg.com/@glidejs/glide@3.5.0/dist/glide.min.js").AsTask();

        }
        await JSRuntime.InvokeVoidAsync($"createGlideInstance");
        await base.OnAfterRenderAsync(firstRender);
    }

    public ValueTask DisposeAsync()
    {
        return _glider is not null
            ? _glider.DisposeAsync()
            : ValueTask.CompletedTask;
    }
}
