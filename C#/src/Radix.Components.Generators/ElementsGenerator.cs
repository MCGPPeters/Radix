
using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Radix.Components.Generators
{
    /// <summary>
    /// A generator for element functions
    /// </summary>
    [Generator]
    public class ElementsGenerator : ISourceGenerator
    {
        private string GetElementMethods(string tagName) =>
            $@"
                public static Node {tagName}(params Node[] nodes) =>
                    element(nameof({tagName}), Array.Empty<IAttribute>(), nodes);

                public static Node {tagName}(params IAttribute[] attributes) =>
                    element(nameof({tagName}), attributes, Array.Empty<Node>());

                public static Node {tagName}(IEnumerable<IAttribute> attributes, params Node[] children) =>
                    element(nameof({tagName}), attributes, children);

                public static Node {tagName}(IAttribute attribute, params Node[] children) =>
                    element(nameof({tagName}), new []{{ attribute }}, children);
            ";


        public void Execute(GeneratorExecutionContext context)
        {
            string[] tagNames = new string[]
            {
                "a", "abbr", "acronym", "address", "applet", "area", "article", "aside", "audio", "b", "@base", "basefont",
                "bdi", "bdo", "big", "br", "button", "canvas", "caption", "center", "cite", "code", "col", "colgroup", "content",
                "data", "datalist", "dd", "del", "details", "dfn", "dialog", "dir", "div", "dl", "dt", "em", "embed", "fieldset", "figcaption",
                "figure", "font", "footer", "form", "frame", "fraeset", "h1", "h2", "h3", "h4", "h5", "h6", "head", "header", "hr", "html",
                "i", "iframe", "img", "input", "ïns", "kbd", "label", "legend", "li", "link", "main", "map", "mark", "menu", "menuitem",
                "meta", "meter", "nav", "noembed", "noframes", "noscript", "@object", "ol", "optgroup", "option", "output", "p", "param",
                "picture", "pre", "progress", "q", "rb", "rp", "rt", "rtc", "ruby", "s", "samp", "script", "section", "select", "shadow", "slot",
                "small", "source", "span", "strike", "strong", "style", "sub", "summary", "sup", "svg", "table", "tbody", "td", "template", "text",
                "textarea", "tfoot", "th", "thead", "time", "title", "tr", "track", "tt", "u", "ul", "var", "video", "wbr"
            };

            var methodsStringBuilder = new StringBuilder();

            for (int i = 0; i < tagNames.Length; i++)
            {
                methodsStringBuilder.Append(GetElementMethods(tagNames[i]));
                methodsStringBuilder.Append(Environment.NewLine);
            }

            string classText =
            $@"
                using System;

                namespace Radix.Components.Html;

                public static class Elements
                {{
                    public static Node text(string text) => new Text(text);

                    public static Concat concat(params Node[] nodes) => new(nodes);

                    public static Element element(Name name, IEnumerable<IAttribute> attributes, params Node[] children) => new(name, attributes, children);

                    {methodsStringBuilder}
                }}
            ";

            // Register the attribute source
            context.AddSource("Elements", classText);
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
