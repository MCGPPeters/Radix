
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
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

        protected override Interaction.Update<RegularModel, RegularCommand> Update => (model, _) => Task.FromResult(model);

        protected override View<RegularModel, RegularCommand> View =>
            async (model, dispatch) =>
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
                    await Task.FromResult(div
                    (
                        new[]
                        {
                            @class((NodeId)2, $"{CardCssClassName}"), id((NodeId)3, model.Id ?? "")
                        },

                        new Node[]
                        { 
                            div
                            (
                                new Attribute[]
                                {
                                    @class((NodeId)5, PrimaryActionCssClassName),
                                    tabindex((NodeId)6, $"{model.TabIndex}")
                                },
                                Array.Empty<Node>()
                            )
                        }
                    ));
            };
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


