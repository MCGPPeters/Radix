using System.Runtime.CompilerServices;
using Radix.Web.Html.Data.Names.Attributes;
using static Radix.Interaction.Attribute;

namespace Radix.Interaction.Web;

public static class Attributes
{
    public static Data.Attribute attribute(string name, string[] values, [CallerLineNumber] int nodeId = 0) =>
        Create(name, values, nodeId);

    public static Data.Attribute accept(string[] values, [CallerLineNumber] int nodeId = 0) =>
        Create<accept>(values, nodeId);

    public static Data.Attribute acceptCharset(string[] values, [CallerLineNumber] int nodeId = 0) => Create<acceptCharset>(values, nodeId);

    public static Data.Attribute accesskey(string[] values, [CallerLineNumber] int nodeId = 0) => Create<accesskey>(values, nodeId);

    public static Data.Attribute action(string[] values, [CallerLineNumber] int nodeId = 0) => Create<action>(values, nodeId);

    public static Data.Attribute allow(string[] values, [CallerLineNumber] int nodeId = 0) => Create<allow>(values, nodeId);

    public static Data.Attribute alt(string[] values, [CallerLineNumber] int nodeId = 0) => Create<alt>(values, nodeId);

    public static Data.Attribute aria_label(string[] values, [CallerLineNumber] int nodeId = 0) => Create<ariaLabel>(values, nodeId);
    public static Data.Attribute aria_hidden(string[] values, [CallerLineNumber] int nodeId = 0) => Create<ariaHidden>(values, nodeId);

    public static Data.Attribute async(string[] values, [CallerLineNumber] int nodeId = 0) => Create<async>(values, nodeId);

    public static Data.Attribute autocapitalize(string[] values, [CallerLineNumber] int nodeId = 0) => Create<autocapitalize>(values, nodeId);

    public static Data.Attribute autocomplete(string[] values, [CallerLineNumber] int nodeId = 0) => Create<autocomplete>(values, nodeId);

    public static Data.Attribute autofocus(string[] values, [CallerLineNumber] int nodeId = 0) => Create<autofocus>(values, nodeId);

    public static Data.Attribute autoplay(string[] values, [CallerLineNumber] int nodeId = 0) => Create<autoplay>(values, nodeId);

    public static Data.Attribute @class(string[] values, [CallerLineNumber] int nodeId = 0) => Create<@class>(values, nodeId);
    public static Data.Attribute charset(string[] values, [CallerLineNumber] int nodeId = 0) => Create<charset>(values, nodeId);

    public static Data.Attribute @checked(string[] values, [CallerLineNumber] int nodeId = 0) => Create<@checked>(values, nodeId);
    public static Data.Attribute cite(string[] values, [CallerLineNumber] int nodeId = 0) => Create<cite>(values, nodeId);

    public static Data.Attribute color(string[] values, [CallerLineNumber] int nodeId = 0) => Create<color>(values, nodeId);

    public static Data.Attribute cols(string[] values, [CallerLineNumber] int nodeId = 0) => Create<cols>(values, nodeId);

    public static Data.Attribute colspan(string[] values, [CallerLineNumber] int nodeId = 0) => Create<colspan>(values, nodeId);

    public static Data.Attribute content(string[] values, [CallerLineNumber] int nodeId = 0) => Create<content>(values, nodeId);

    public static Data.Attribute contenteditable(string[] values, [CallerLineNumber] int nodeId = 0) => Create<contenteditable>(values, nodeId);

    public static Data.Attribute controls(string[] values, [CallerLineNumber] int nodeId = 0) => Create<controls>(values, nodeId);

    public static Data.Attribute coords(string[] values, [CallerLineNumber] int nodeId = 0) => Create<coords>(values, nodeId);

    public static Data.Attribute crossorigin(string[] values, [CallerLineNumber] int nodeId = 0) => Create<crossorigin>(values, nodeId);

    public static Data.Attribute data(string[] values, [CallerLineNumber] int nodeId = 0) => Create<data>(values, nodeId);

    public static Data.Attribute datetime(string[] values, [CallerLineNumber] int nodeId = 0) => Create<datetime>(values, nodeId);

    public static Data.Attribute decoding(string[] values, [CallerLineNumber] int nodeId = 0) => Create<decoding>(values, nodeId);

    public static Data.Attribute @default(string[] values, [CallerLineNumber] int nodeId = 0) => Create<@default>(values, nodeId);
    public static Data.Attribute defer(string[] values, [CallerLineNumber] int nodeId = 0) => Create<defer>(values, nodeId);

    public static Data.Attribute dir(string[] values, [CallerLineNumber] int nodeId = 0) => Create<dir>(values, nodeId);

    public static Data.Attribute dirname(string[] values, [CallerLineNumber] int nodeId = 0) => Create<dirname>(values, nodeId);

    public static Data.Attribute disabled(string[] values, [CallerLineNumber] int nodeId = 0) => Create<disabled>(values, nodeId);

    public static Data.Attribute download(string[] values, [CallerLineNumber] int nodeId = 0) => Create<download>(values, nodeId);

    public static Data.Attribute draggable(string[] values, [CallerLineNumber] int nodeId = 0) => Create<draggable>(values, nodeId);

    public static Data.Attribute enctype(string[] values, [CallerLineNumber] int nodeId = 0) => Create<enctype>(values, nodeId);

    public static Data.Attribute @for(string[] values, [CallerLineNumber] int nodeId = 0) => Create<@for>(values, nodeId);
    public static Data.Attribute form(string[] values, [CallerLineNumber] int nodeId = 0) => Create<form>(values, nodeId);

    public static Data.Attribute formaction(string[] values, [CallerLineNumber] int nodeId = 0) => Create<formaction>(values, nodeId);

    public static Data.Attribute headers(string[] values, [CallerLineNumber] int nodeId = 0) => Create<headers>(values, nodeId);

    public static Data.Attribute height(string[] values, [CallerLineNumber] int nodeId = 0) => Create<height>(values, nodeId);

    public static Data.Attribute hidden(string[] values, [CallerLineNumber] int nodeId = 0) => Create<hidden>(values, nodeId);

    public static Data.Attribute high(string[] values, [CallerLineNumber] int nodeId = 0) => Create<high>(values, nodeId);

    public static Data.Attribute href(string[] values, [CallerLineNumber] int nodeId = 0) => Create<href>(values, nodeId);

    public static Data.Attribute hreflang(string[] values, [CallerLineNumber] int nodeId = 0) => Create<hreflang>(values, nodeId);

    public static Data.Attribute httpequiv(string[] values, [CallerLineNumber] int nodeId = 0) => Create<httpEquiv>(values, nodeId);


    public static Data.Attribute id(string[] values, [CallerLineNumber] int nodeId = 0) => Create<id>(values, nodeId);

    public static Data.Attribute ismap(string[] values, [CallerLineNumber] int nodeId = 0) => Create<ismap>(values, nodeId);

    public static Data.Attribute itemprop(string[] values, [CallerLineNumber] int nodeId = 0) => Create<itemprop>(values, nodeId);

    public static Data.Attribute kind(string[] values, [CallerLineNumber] int nodeId = 0) => Create<kind>(values, nodeId);

    public static Data.Attribute label(string[] values, [CallerLineNumber] int nodeId = 0) => Create<label>(values, nodeId);

    public static Data.Attribute lang(string[] values, [CallerLineNumber] int nodeId = 0) => Create<lang>(values, nodeId);

    public static Data.Attribute list(string[] values, [CallerLineNumber] int nodeId = 0) => Create<list>(values, nodeId);

    public static Data.Attribute loop(string[] values, [CallerLineNumber] int nodeId = 0) => Create<loop>(values, nodeId);

    public static Data.Attribute low(string[] values, [CallerLineNumber] int nodeId = 0) => Create<low>(values, nodeId);

    public static Data.Attribute max(string[] values, [CallerLineNumber] int nodeId = 0) => Create<max>(values, nodeId);

    public static Data.Attribute maxlength(string[] values, [CallerLineNumber] int nodeId = 0) => Create<maxlength>(values, nodeId);

    public static Data.Attribute media(string[] values, [CallerLineNumber] int nodeId = 0) => Create<media>(values, nodeId);

    public static Data.Attribute method(string[] values, [CallerLineNumber] int nodeId = 0) => Create<method>(values, nodeId);

    public static Data.Attribute min(string[] values, [CallerLineNumber] int nodeId = 0) => Create<min>(values, nodeId);

    public static Data.Attribute minlength(string[] values, [CallerLineNumber] int nodeId = 0) => Create<minlength>(values, nodeId);

    public static Data.Attribute multiple(string[] values, [CallerLineNumber] int nodeId = 0) => Create<multiple>(values, nodeId);

    public static Data.Attribute muted(string[] values, [CallerLineNumber] int nodeId = 0) => Create<muted>(values, nodeId);

    public static Data.Attribute name(string[] values, [CallerLineNumber] int nodeId = 0) => Create<name>(values, nodeId);

    public static Data.Attribute novalidate(string[] values, [CallerLineNumber] int nodeId = 0) => Create<novalidate>(values, nodeId);

    public static Data.Attribute open(string[] values, [CallerLineNumber] int nodeId = 0) => Create<open>(values, nodeId);

    public static Data.Attribute optimum(string[] values, [CallerLineNumber] int nodeId = 0) => Create<optimum>(values, nodeId);

    public static Data.Attribute pattern(string[] values, [CallerLineNumber] int nodeId = 0) => Create<pattern>(values, nodeId);

    public static Data.Attribute ping(string[] values, [CallerLineNumber] int nodeId = 0) => Create<ping>(values, nodeId);

    public static Data.Attribute placeholder(string[] values, [CallerLineNumber] int nodeId = 0) => Create<placeholder>(values, nodeId);

    public static Data.Attribute poster(string[] values, [CallerLineNumber] int nodeId = 0) => Create<poster>(values, nodeId);

    public static Data.Attribute preload(string[] values, [CallerLineNumber] int nodeId = 0) => Create<preload>(values, nodeId);

    public static Data.Attribute @readonly(string[] values, [CallerLineNumber] int nodeId = 0) => Create<@readonly>(values, nodeId);
    public static Data.Attribute rel(string[] values, [CallerLineNumber] int nodeId = 0) => Create<rel>(values, nodeId);
    /// <summary>
    /// note : a type may not be named required, hence _required
    /// </summary>
    public static Data.Attribute _required(string[] values, [CallerLineNumber] int nodeId = 0) => Create<_required>(values, nodeId);
    public static Data.Attribute reversed(string[] values, [CallerLineNumber] int nodeId = 0) => Create<reversed>(values, nodeId);

    public static Data.Attribute rows(string[] values, [CallerLineNumber] int nodeId = 0) => Create<rows>(values, nodeId);

    public static Data.Attribute rowspan(string[] values, [CallerLineNumber] int nodeId = 0) => Create<rowspan>(values, nodeId);

    public static Data.Attribute sandbox(string[] values, [CallerLineNumber] int nodeId = 0) => Create<sandbox>(values, nodeId);

    public static Data.Attribute scope(string[] values, [CallerLineNumber] int nodeId = 0) => Create<scope>(values, nodeId);

    public static Data.Attribute shape(string[] values, [CallerLineNumber] int nodeId = 0) => Create<shape>(values, nodeId);

    public static Data.Attribute size(string[] values, [CallerLineNumber] int nodeId = 0) => Create<size>(values, nodeId);

    public static Data.Attribute sizes(string[] values, [CallerLineNumber] int nodeId = 0) => Create<sizes>(values, nodeId);

    public static Data.Attribute slot(string[] values, [CallerLineNumber] int nodeId = 0) => Create<slot>(values, nodeId);

    public static Data.Attribute span(string[] values, [CallerLineNumber] int nodeId = 0) => Create<span>(values, nodeId);

    public static Data.Attribute spellcheck(string[] values, [CallerLineNumber] int nodeId = 0) => Create<spellcheck>(values, nodeId);

    public static Data.Attribute src(string[] values, [CallerLineNumber] int nodeId = 0) => Create<src>(values, nodeId);

    public static Data.Attribute srcdoc(string[] values, [CallerLineNumber] int nodeId = 0) => Create<srcdoc>(values, nodeId);

    public static Data.Attribute srclang(string[] values, [CallerLineNumber] int nodeId = 0) => Create<srclang>(values, nodeId);

    public static Data.Attribute srcset(string[] values, [CallerLineNumber] int nodeId = 0) => Create<srcset>(values, nodeId);

    public static Data.Attribute start(string[] values, [CallerLineNumber] int nodeId = 0) => Create<start>(values, nodeId);

    public static Data.Attribute step(string[] values, [CallerLineNumber] int nodeId = 0) => Create<step>(values, nodeId);

    public static Data.Attribute tabindex(string[] values, [CallerLineNumber] int nodeId = 0) => Create<tabindex>(values, nodeId);

    public static Data.Attribute target(string[] values, [CallerLineNumber] int nodeId = 0) => Create<target>(values, nodeId);

    public static Data.Attribute title(string[] values, [CallerLineNumber] int nodeId = 0) => Create<title>(values, nodeId);

    public static Data.Attribute translate(string[] values, [CallerLineNumber] int nodeId = 0) => Create<translate>(values, nodeId);

    public static Data.Attribute type(string[] values, [CallerLineNumber] int nodeId = 0) => Create<type>(values, nodeId);

    public static Data.Attribute usemap(string[] values, [CallerLineNumber] int nodeId = 0) => Create<usemap>(values, nodeId);

    public static Data.Attribute value(string[] values, [CallerLineNumber] int nodeId = 0) => Create<value>(values, nodeId);

    public static Data.Attribute width(string[] values, [CallerLineNumber] int nodeId = 0) => Create<width>(values, nodeId);

    public static Data.Attribute wrap(string[] values, [CallerLineNumber] int nodeId = 0) => Create<wrap>(values, nodeId);
}
