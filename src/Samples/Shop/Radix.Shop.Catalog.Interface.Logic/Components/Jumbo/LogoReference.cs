using Radix.Components;
using Radix.Interaction;
using Radix.Interaction.Data;

namespace Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;

/// <summary>
///
/// </summary>
public class LogoReference : Component<LogoReferenceModel, LogoReferenceCommand>
{
    protected override View<LogoReferenceModel, LogoReferenceCommand> View =>
        async (model, dispatch) =>
            img
            (
                (NodeId)1,
                height((AttributeId)1, "45"),
                width((AttributeId)2, "45"),
                src((AttributeId)3, "https://www.jumbo.com/INTERSHOP/static/WFS/Jumbo-Grocery-Site/-/-/nl_NL/images/favicon.ico")
            );

    protected override Interaction.Update<LogoReferenceModel, LogoReferenceCommand> Update => (model, _) => Task.FromResult(model);
        
}
