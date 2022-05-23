
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Radix.Web.Css.Data;
using Radix.Web.Css.Data.Dimensions;
using Radix.Web.Css.Data.Units.Length.Absolute;
using Radix.Web.Css.Data.Units.Length.Relative;

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

        protected override Interact<RegularModel, RegularCommand> Interact =>
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
                    div
                    (
                        (NodeId)1,
                        new[]
                        {
                            @class((AttributeId)1, $"{CardCssClassName}"),
                            id((AttributeId)2, model.Id ?? "")
                        },
                        div
                        (
                            (NodeId)2,
                            new[]
                            {
                                @class((AttributeId)3, PrimaryActionCssClassName),
                                tabindex((AttributeId)3, $"{model.TabIndex}")
                            }
                        )
                    );
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


