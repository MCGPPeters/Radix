using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Components;

namespace Radix.Web.Html.Data;

public static class bind
{

    private const string InputEventName = "input";
    private const string ChangeEventName = "change";

    private static Attribute binder<T>(string name, T currentValue, Action<T> callback, CultureInfo cultureInfo) => new ExplicitAttribute(
        name,
        (builder, sequence, receiver) =>
        {
            builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.CreateBinder(receiver, callback, currentValue, cultureInfo));
            return sequence + 1;
        });

    public static Attribute input<T>(T value, Action<T> callback, CultureInfo cultureInfo = default) => binder(InputEventName, value, callback, cultureInfo);

    public static Attribute change<T>(T value, Action<T> callback, CultureInfo cultureInfo = default) => binder(ChangeEventName, value, callback, cultureInfo);
}
