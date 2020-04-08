using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Radix.Blazor.Html
{
    public static class bind
    {

        private const string InputEventName = "input";
        private const string ChangeEventName = "change";

        public static IAttribute binder<T>(Name name, T currentValue, Action<T> callback, CultureInfo cultureInfo)
        {
            return new ExplicitAttribute(
                name,
                (builder, sequence, receiver) =>
                {
                    builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.CreateBinder(receiver, callback, currentValue, cultureInfo));
                    return sequence + 1;
                });
        }

        public static IAttribute input<T>(T value, Action<T> callback, CultureInfo cultureInfo = default)
        {
            return binder(InputEventName, value, callback, cultureInfo);
        }

        public static IAttribute change<T>(T value, Action<T> callback, CultureInfo cultureInfo = default)
        {
            return binder(ChangeEventName, value, callback, cultureInfo);
        }
    }
}
