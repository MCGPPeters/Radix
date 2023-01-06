using System.Runtime.CompilerServices;
using Radix.Web.Html.Data.Names.Attributes;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;
using Radix.Web.Html.Data;

namespace Radix.Interaction.Web.Components;

public static class Attributes
{
    public static Data.Attribute<string> accept(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<accept>(values, nodeId);

    public static Data.Attribute<string> acceptCharset(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<acceptCharset>(values, nodeId);

    public static Data.Attribute<string> accesskey(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<accesskey>(values, nodeId);

    public static Data.Attribute<string> action(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<action>(values, nodeId);

    public static Data.Attribute<string> allow(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<allow>(values, nodeId);

    public static Data.Attribute<string> alt(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<alt>(values, nodeId);

    public static Data.Attribute<string> aria_label(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<ariaLabel>(values, nodeId);

    public static Data.Attribute<string> async(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<async>(values, nodeId);

    public static Data.Attribute<string> autocapitalize(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<autocapitalize>(values, nodeId);

    public static Data.Attribute<string> autocomplete(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<autocomplete>(values, nodeId);

    public static Data.Attribute<string> autofocus(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<autofocus>(values, nodeId);

    public static Data.Attribute<string> autoplay(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<autoplay>(values, nodeId);

    public static Data.Attribute<string> @class(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<@class>(values, nodeId);

    public static Data.Attribute<string> charset(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<charset>(values, nodeId);

    public static Data.Attribute<string> @checked(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<@checked>(values, nodeId);

    public static Data.Attribute<string> cite(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<cite>(values, nodeId);

    public static Data.Attribute<string> color(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<color>(values, nodeId);

    public static Data.Attribute<string> cols(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<cols>(values, nodeId);

    public static Data.Attribute<string> colspan(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<colspan>(values, nodeId);

    public static Data.Attribute<string> content(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<content>(values, nodeId);

    public static Data.Attribute<string> contenteditable(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<contenteditable>(values, nodeId);

    public static Data.Attribute<string> controls(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<controls>(values, nodeId);

    public static Data.Attribute<string> coords(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<coords>(values, nodeId);

    public static Data.Attribute<string> crossorigin(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<crossorigin>(values, nodeId);

    public static Data.Attribute<string> data(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<data>(values, nodeId);

    public static Data.Attribute<string> datetime(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<datetime>(values, nodeId);

    public static Data.Attribute<string> decoding(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<decoding>(values, nodeId);

    public static Data.Attribute<string> @default(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<@default>(values, nodeId);

    public static Data.Attribute<string> defer(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<defer>(values, nodeId);

    public static Data.Attribute<string> dir(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<dir>(values, nodeId);

    public static Data.Attribute<string> dirname(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<dirname>(values, nodeId);

    public static Data.Attribute<string> disabled(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<disabled>(values, nodeId);

    public static Data.Attribute<string> download(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<download>(values, nodeId);

    public static Data.Attribute<string> draggable(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<draggable>(values, nodeId);

    public static Data.Attribute<string> enctype(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<enctype>(values, nodeId);

    public static Data.Attribute<string> @for(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<@for>(values, nodeId);

    public static Data.Attribute<string> form(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<form>(values, nodeId);

    public static Data.Attribute<string> formaction(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<formaction>(values, nodeId);

    public static Data.Attribute<string> headers(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<headers>(values, nodeId);

    public static Data.Attribute<string> height(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<height>(values, nodeId);

    public static Data.Attribute<string> hidden(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<hidden>(values, nodeId);

    public static Data.Attribute<string> high(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<high>(values, nodeId);

    public static Data.Attribute<string> href(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<href>(values, nodeId);

    public static Data.Attribute<string> hreflang(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<hreflang>(values, nodeId);

    public static Data.Attribute<string> httpequiv(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<httpEquiv>(values, nodeId);


    public static Data.Attribute<string> id(string?[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<id>(values, nodeId);

    public static Data.Attribute<string> ismap(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<ismap>(values, nodeId);

    public static Data.Attribute<string> itemprop(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<itemprop>(values, nodeId);

    public static Data.Attribute<string> kind(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<kind>(values, nodeId);

    public static Data.Attribute<string> label(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<label>(values, nodeId);

    public static Data.Attribute<string> lang(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<lang>(values, nodeId);

    public static Data.Attribute<string> list(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<list>(values, nodeId);

    public static Data.Attribute<string> loop(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<loop>(values, nodeId);

    public static Data.Attribute<string> low(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<low>(values, nodeId);

    public static Data.Attribute<string> max(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<max>(values, nodeId);

    public static Data.Attribute<string> maxlength(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<maxlength>(values, nodeId);

    public static Data.Attribute<string> media(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<media>(values, nodeId);

    public static Data.Attribute<string> method(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<method>(values, nodeId);

    public static Data.Attribute<string> min(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<min>(values, nodeId);

    public static Data.Attribute<string> minlength(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<minlength>(values, nodeId);

    public static Data.Attribute<string> multiple(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<multiple>(values, nodeId);

    public static Data.Attribute<string> muted(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<muted>(values, nodeId);

    public static Data.Attribute<string> name(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<name>(values, nodeId);

    public static Data.Attribute<string> novalidate(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<novalidate>(values, nodeId);

    public static Data.Attribute<string> open(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<open>(values, nodeId);

    public static Data.Attribute<string> optimum(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<optimum>(values, nodeId);

    public static Data.Attribute<string> pattern(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<pattern>(values, nodeId);

    public static Data.Attribute<string> ping(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<ping>(values, nodeId);

    public static Data.Attribute<string> placeholder(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<placeholder>(values, nodeId);

    public static Data.Attribute<string> poster(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<poster>(values, nodeId);

    public static Data.Attribute<string> preload(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<preload>(values, nodeId);

    public static Data.Attribute<string> @readonly(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<@readonly>(values, nodeId);

    public static Data.Attribute<string> rel(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<rel>(values, nodeId);

    //public static Data.Attribute<string> @required(string[] values, [CallerLineNumber] int nodeId = 0) 
    //    => attribute<@required>(values, nodeId);

    public static Data.Attribute<string> reversed(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<reversed>(values, nodeId);

    public static Data.Attribute<string> rows(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<rows>(values, nodeId);

    public static Data.Attribute<string> rowspan(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<rowspan>(values, nodeId);

    public static Data.Attribute<string> sandbox(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<sandbox>(values, nodeId);

    public static Data.Attribute<string> scope(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<scope>(values, nodeId);

    public static Data.Attribute<string> shape(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<shape>(values, nodeId);

    public static Data.Attribute<string> size(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<size>(values, nodeId);

    public static Data.Attribute<string> sizes(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<sizes>(values, nodeId);

    public static Data.Attribute<string> slot(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<slot>(values, nodeId);

    public static Data.Attribute<string> span(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<span>(values, nodeId);

    public static Data.Attribute<string> spellcheck(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<spellcheck>(values, nodeId);

    public static Data.Attribute<string> src(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<src>(values, nodeId);

    public static Data.Attribute<string> srcdoc(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<srcdoc>(values, nodeId);

    public static Data.Attribute<string> srclang(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<srclang>(values, nodeId);

    public static Data.Attribute<string> srcset(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<srcset>(values, nodeId);

    public static Data.Attribute<string> start(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<start>(values, nodeId);

    public static Data.Attribute<string> step(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<step>(values, nodeId);

    public static Data.Attribute<string> tabindex(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<tabindex>(values, nodeId);

    public static Data.Attribute<string> target(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<target>(values, nodeId);

    public static Data.Attribute<string> title(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<title>(values, nodeId);

    public static Data.Attribute<string> translate(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<translate>(values, nodeId);

    public static Data.Attribute<string> type(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<type>(values, nodeId);

    public static Data.Attribute<string> usemap(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<usemap>(values, nodeId);

    public static Data.Attribute<string> value(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<value>(values, nodeId);

    public static Data.Attribute<string> width(string[] values, [CallerLineNumber] int nodeId = 0) 
        => attribute<width>(values, nodeId);

    public static Data.Attribute<string> wrap(string[] values, [CallerLineNumber] int nodeId = 0)
        => attribute<wrap>(values, nodeId);

    public static Data.Attribute<string> attribute<T>(string[] values, [CallerLineNumber]int id = 0)
        where T : AttributeName, Literal<T> =>
        new Attribute<T>((NodeId)id, values);

    public static Attribute attribute(string name, string[] values, [CallerLineNumber] int id = 0) =>
        new Data.Attribute<string>((NodeId)id, name, values);
}
