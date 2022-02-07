using Radix.Components.Html;
using Radix.Web.Css.Data;
using Radix.Web.Css.Data.Dimensions;
using Radix.Web.Css.Data.Units.Length.Absolute;
using Radix.Web.Css.Data.Units.Length.Relative;

namespace Radix.Components.Material._3._2._0.Card
{
    /// <summary>
    /// A default card without outline and with elevation
    /// </summary>
    public class Regular : Component<RegularViewModel>
    {
        private const string CardCssClassName = "mdc-card";

        private const string PrimaryActionCssClassName = "mdc-card__primary-action";



        protected override Node View(RegularViewModel currentViewModel)
        {
            
            return
                div
                (
                    new[]
                    {
                        @class($"{CardCssClassName}"),
                        id(currentViewModel.Id ?? "")
                    },
                    div
                    (
                        new[]
                        {
                            @class(PrimaryActionCssClassName),
                            tabindex($"{currentViewModel.TabIndex}")
                        }                        
                    )
                );
        }

        private Node Styles(RegularViewModel currentViewModel) =>
        style
        (
            // todo : figure out media query in style element for search box font-size
            text
            (
                $@"
                    .my-card-content {{
                      padding: {currentViewModel.Padding.ToString()};
                    }}

                    .my-card-dimensions {{
                      {currentViewModel.Height.ToString()};
                      width: {currentViewModel.Width.ToString()};
                    }}
                "
            )
        );
    }

    public record RegularViewModel
        (
            Length Padding,
            Web.Css.Data.Declarations.Height.Declaration Height,
            Web.Css.Data.Declarations.Width.Declaration Width,
            string? Id,
            int TabIndex,
            string? HeaderText
        ) : ViewModel;

    
}


