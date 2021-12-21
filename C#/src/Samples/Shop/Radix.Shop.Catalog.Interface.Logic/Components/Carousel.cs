using Microsoft.JSInterop;
using Radix.Components;
using Radix.Components.Html;
using Microsoft.AspNetCore.Components.Web.Extensions.Head;
using Tsheap.Com.Components;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public class Carousel : Component<CarouselViewModel>, IAsyncDisposable
{
    private IJSObjectReference? _glider;


    protected override Node View(CarouselViewModel currentViewModel) => concat
            (
                component<Link>
                (
                    rel("stylesheet"),
                    href("https://unpkg.com/@glidejs/glide@3.5.0/dist/css/glide.core.min.css")
                ),
                script
                (
                    text($"function createGlideInstance() {{ return new Glide('.{ViewModel.Id}').mount()}};")
                ),
                div
                (
                    @class(currentViewModel.Id),
                    concat
                    (
                        div
                        (
                            new[]
                            {
                                @class("glide__track"),
                                attribute("data-glide-el", "track")
                            },
                            ul
                            (
                                @class("glide__slides"),
                                currentViewModel.Children.Select
                                (
                                    element =>
                                        li
                                        (
                                            @class("glide__slide"),
                                            element
                                        )
                                ).ToArray()
                            )
                        ),
                        div
                        (
                            new[]
                            {
                                @class("glide__arrows"),
                                attribute("data-glide-el", "controls")
                            },
                            button
                            (
                                new[]
                                {
                                    @class("slider__arrow slider__arrow--next glide__arrow glide__arrow--left"),
                                    attribute("data-glide-dir", "<")
                                },
                                svg
                                (
                                    new[]
                                    {
                                        attribute("xmlns", "http://www.w3.org/2000/svg"),
                                        width("18"),
                                        height("18"),
                                        attribute("viewBox", "0 0 24 24")
                                    },
                                    element
                                    (
                                        "path",
                                        new[]
                                        {
                                            attribute("d", "M0 12l10.975 11 2.848-2.828-6.176-6.176H24v-3.992H7.646l6.176-6.176L10.975 1 0 12z")
                                        }
                                    )
                                )
                            ),
                            button
                            (
                                new[]
                                {
                                    @class("slider__arrow slider__arrow--next glide__arrow glide__arrow--right"),
                                    attribute("data-glide-dir", ">")
                                },
                                svg
                                (
                                    new[]
                                    {
                                        attribute("xmlns", "http://www.w3.org/2000/svg"),
                                        width("18"),
                                        height("18"),
                                        attribute("viewBox", "0 0 24 24")
                                    },
                                    element
                                    (
                                        "path",
                                        new[]
                                        {
                                            attribute("d", "M13.025 1l-2.847 2.828 6.176 6.176h-16.354v3.992h16.354l-6.176 6.176 2.847 2.828 10.975-11z")
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
