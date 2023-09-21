//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HtmlAgilityPack;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Rendering;
//using Radix.Interaction.Components;
//using Radix.Interaction.Components.Nodes;
//using Radix.Interaction.Data;
//using Radix.Interaction.Web.Components.Nodes;
//using Attribute = Radix.Interaction.Data.Attribute;

//namespace Radix.Interaction.Web;

//internal static class HTMLRenderer
//{
//    public static HtmlDocument ToHtmlDocument(this Node node)
//    {
//        var htmlDocument = new HtmlDocument();
//        switch (node)
//        {

//            case Text(var text, var id):
//                htmlDocument.CreateTextNode(text);
//                break;
//            case HtmlString(var html, var id):
//                var htmlElement = htmlDocument.CreateElement(id.ToString());
//                htmlElement.InnerHtml = html;
//                htmlElement.Id = id.ToString();
//                htmlDocument.do
//                break;
//            case Concat nodes:
//                foreach (Node n in nodes)
//                {
//                    Render?.Invoke(currentComponent, builder)(n);
//                }

//                break;
//            case ElementName element:
//                builder.OpenRegion(element.NodeId);
//                builder.OpenElement(element.NodeId, element.Name);

//                Attributes(currentComponent, builder, element.Attributes);
//                foreach (Node child in element.Children)
//                {
//                    Render?.Invoke(currentComponent, builder)(child);
//                }

//                builder.AddElementReferenceCapture(int.MaxValue,
//                    elementReference => element.ElementReference = elementReference);

//                builder.CloseElement();
//                builder.CloseRegion();
//                break;
//            case Component component:
//                builder.OpenRegion(component.NodeId);
//                builder.OpenComponent(component.NodeId, component.Type);
//                Attributes(currentComponent, builder, component.Attributes);
//                if (component.Children.Any())
//                {
//                    RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
//                        component.NodeId,
//                        renderTreeBuilder =>
//                        {
//                            foreach (Node elementChild in component.Children)
//                            {
//                                Render?.Invoke(currentComponent, renderTreeBuilder)(elementChild);
//                            }
//                        });
//                    builder.AddAttribute(1, "ChildContent", fragment);
//                }

//                builder.CloseComponent();
//                builder.CloseRegion();
//                break;
//        }
//        new HtmlDocument();
//    }


//}



//public static class Prelude
//{
//    public static Func<object, RenderTreeBuilder, Render> Render { get; } =
//        (currentComponent, builder) =>
//            node =>
//            {
//                switch (node)
//                {

//                    case Text(var text, var id):
//                        builder.AddContent(id, text);
//                        break;
//                    case HtmlString(var html, var id):
//                        builder.AddMarkupContent(id, html);
//                        break;
//                    case Concat nodes:
//                        foreach (Node n in nodes)
//                        {
//                            Render?.Invoke(currentComponent, builder)(n);
//                        }

//                        break;
//                    case ElementName element:
//                        builder.OpenRegion(element.NodeId);
//                        builder.OpenElement(element.NodeId, element.Name);

//                        Attributes(currentComponent, builder, element.Attributes);
//                        foreach (Node child in element.Children)
//                        {
//                            Render?.Invoke(currentComponent, builder)(child);
//                        }

//                        builder.AddElementReferenceCapture(int.MaxValue,
//                            elementReference => element.ElementReference = elementReference);

//                        builder.CloseElement();
//                        builder.CloseRegion();
//                        break;
//                    case Component component:
//                        builder.OpenRegion(component.NodeId);
//                        builder.OpenComponent(component.NodeId, component.Type);
//                        Attributes(currentComponent, builder, component.Attributes);
//                        if (component.Children.Any())
//                        {
//                            RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
//                                component.NodeId,
//                                renderTreeBuilder =>
//                                {
//                                    foreach (Node elementChild in component.Children)
//                                    {
//                                        Render?.Invoke(currentComponent, renderTreeBuilder)(elementChild);
//                                    }
//                                });
//                            builder.AddAttribute(1, "ChildContent", fragment);
//                        }

//                        builder.CloseComponent();
//                        builder.CloseRegion();
//                        break;
//                }
//            };

//    private static void Attributes(object currentComponent, RenderTreeBuilder builder,
//        IEnumerable<Attribute> attributes)
//    {
//        foreach (Data.Attribute attribute in attributes)
//        {
//            switch (attribute)
//            {
//                case ComponentAttribute(var id, var name, var value):
//                    builder.AddAttribute(id, name, value);
//                    break;
//                case ExplicitAttribute explicitAttribute:
//                    explicitAttribute.Factory(builder, explicitAttribute.NodeId, currentComponent);
//                    break;
//                case Data.Attribute<string> htmlAttribute:
//                    if (htmlAttribute.Values.Length != 0)
//                    {
//                        builder.AddAttribute(htmlAttribute.NodeId, htmlAttribute.Name,
//                            htmlAttribute.Values.Aggregate((current, next) => $"{current} {next}"));
//                    }

//                    break;
//            }
//        }
//    }
//}
