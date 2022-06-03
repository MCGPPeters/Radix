using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components;

public static class bind
{

    private const string InputEventName = "input";
    private const string ChangeEventName = "change";

    private static Attribute binder<T>(AttributeId attributeId, string name, T currentValue, Action<T> callback, CultureInfo cultureInfo) => new ExplicitAttribute(
        attributeId,
        name,
        (builder, sequence, receiver) =>
        {
            builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.CreateBinder(receiver, callback, currentValue, cultureInfo));
            return attributeId + 1;
        });

    public static Attribute input<T>(AttributeId attributeId, T value, Action<T> callback, CultureInfo cultureInfo = default) =>
        binder(attributeId, InputEventName, value, callback, cultureInfo);

    public static Attribute change<T>(AttributeId attributeId, T value, Action<T> callback, CultureInfo cultureInfo = default) =>
        binder(attributeId, ChangeEventName, value, callback, cultureInfo);
}
