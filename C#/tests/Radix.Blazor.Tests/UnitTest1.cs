using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Radix.Blazor.Html;
using Xunit;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Tests
{
    public class UnitTest1
    {

        private Model Update(Message message, Model model)
        {
            switch (message)
            {
                case SetInput(var text):
                    if (text is object)
                    {
                        string? modelInput = text.ToString();
                        if (modelInput is object)
                        {
                            model.Input = modelInput;
                        }
                    }

                    return model;
                default:
                    return model;
            }
        }


        private Node FormView(IJSRuntime _, Model model, Func<Message, Task> dispatch) => concat(
            h1(new[] {value("Title"), @class("nice")}, text("Title")),
            input(value(model.Input), on.change(async args => await dispatch(new SetInput(args.Value))))
        );

        [Fact]
        public void Test1()
        {

        }
    }

    internal class SetInput : Message
    {
        private readonly object _argsValue;

        public SetInput(object argsValue) => _argsValue = argsValue;

        public void Deconstruct(out object value) => value = _argsValue;
    }

    internal class Model
    {
        public string Input { get; set; }
    }

    internal interface Message
    {
    }
}
