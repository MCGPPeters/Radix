
using System.Runtime.InteropServices.JavaScript;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web;
using Radix.Web.Css.Data;
using Radix.Web.Css.Data.Dimensions;
using Radix.Web.Css.Data.Units.Length.Absolute;
using Radix.Web.Css.Data.Units.Length.Relative;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Components.Material._3._2._0.Card
{
    /// <summary>
    /// A default card without outline and with elevation
    /// </summary>
    public class Regular : Component<RegularModel, RegularCommand>
    {
        private const string CardCssClassName = "mdc-card";

        private const string PrimaryActionCssClassName = "mdc-card__primary-action";

        public override async ValueTask<RegularModel> Update(RegularModel model, RegularCommand _) => model;

        public override Node View(RegularModel model, Func<RegularCommand, Task> dispatch)
        {

            model = model with
            {
                PaddingLeft = new Web.Css.Data.Declarations.PaddingLeft.Length
                {
                    Value = new AbsoluteLength<px>((Number)22)
                }
            };

            model = model with
            {
                PaddingLeft = new Web.Css.Data.Declarations.PaddingLeft.Percentage
                {
                    Value = new Percentage((Number)5)
                }
            };

            return
                div
                (
                    new[]
                    {
                            @class(new []{$"{CardCssClassName}"}), id(new[] { model.Id ?? "" })
                    },

                    new Node[]
                    {
                            div
                            (
                                new Attribute[]
                                {
                                    @class(new[] { PrimaryActionCssClassName }),
                                    tabindex(new[] { $"{model.TabIndex}" })
                                },
                                Array.Empty<Node>()
                            )
                    }
                );
        }
    }

    public record RegularModel
        (
            Web.Css.Data.Declarations.PaddingLeft.Declaration PaddingLeft,
            Web.Css.Data.Declarations.PaddingRight.Declaration PaddingRight,
            Web.Css.Data.Declarations.PaddingTop.Declaration PaddingTop,
            Web.Css.Data.Declarations.PaddingBottom.Declaration PaddingBottom,
            Web.Css.Data.Declarations.Height.Declaration Height,
            Web.Css.Data.Declarations.Width.Declaration Width,
            string? Id,
            int TabIndex,
            string? HeaderText
        );


}


