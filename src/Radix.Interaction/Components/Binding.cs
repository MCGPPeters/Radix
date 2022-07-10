using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components;

public static class bind
{

    private const string InputEventName = "input";
    private const string ChangeEventName = "change";

    private static Attribute binder<T>(NodeId nodeId, string name, T currentValue, Action<T> callback, CultureInfo cultureInfo) => new ExplicitAttribute(
        nodeId,
        name,
        (builder, sequence, receiver) =>
        {
            builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.CreateBinder(receiver, callback, currentValue, cultureInfo));
            return nodeId + 1;
        });

    public static Attribute input<T>(NodeId nodeId, T value, Action<T> callback, CultureInfo cultureInfo = default) =>
        binder(nodeId, InputEventName, value, callback, cultureInfo);

    public static Attribute change<T>(NodeId nodeId, T value, Action<T> callback, CultureInfo cultureInfo = default) =>
        binder(nodeId, ChangeEventName, value, callback, cultureInfo);
}
