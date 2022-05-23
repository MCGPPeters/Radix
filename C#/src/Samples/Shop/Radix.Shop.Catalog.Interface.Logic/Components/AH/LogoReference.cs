using Radix.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;

namespace Radix.Shop.Catalog.Interface.Logic.Components.AH;

/// <summary>
///
/// </summary>
public class LogoReference : Component<LogoReferenceModel, LogoReferenceCommand>
{

    protected override Interact<LogoReferenceModel, LogoReferenceCommand> Interact =>
        async (model, dispatch) =>
            img
            (
                (NodeId)1,
                height((AttributeId)1, "45"),
                width((AttributeId)2, "45"),
                src((AttributeId)3, "/images/ah.svg")
            );

    protected override Interaction.Update<LogoReferenceModel, LogoReferenceCommand> Update => (model, _) => Task.FromResult(model);
}
