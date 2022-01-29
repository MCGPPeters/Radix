using Radix.Web.Html.Data.Names.Attributes;
using Radix.Web.Html.Data;

namespace Radix.Web.Html;

public delegate Data.Attribute attribute(params string[] values);

public static class Attributes
{
    public static attribute accept = values
        => attribute<accept>(values);

    public static attribute acceptCharset = values
        => attribute<acceptCharset>(values);

    public static attribute accesskey = values
        => attribute<accesskey>(values);

    public static attribute action = values
        => attribute<action>(values);

    public static attribute allow = values
        => attribute<allow>(values);

    public static attribute alt = values
        => attribute<alt>(values);

    public static attribute aria_label = values
        => attribute<ariaLabel>(values);

    public static attribute async = values
        => attribute<async>(values);

    public static attribute autocapitalize = values
        => attribute<autocapitalize>(values);

    public static attribute autocomplete = values
        => attribute<autocomplete>(values);

    public static attribute autofocus = values
        => attribute<autofocus>(values);

    public static attribute autoplay = values
        => attribute<autoplay>(values);

    public static attribute @class = values
        => attribute<@class>(values);

    public static attribute charset = values
        => attribute<charset>(values);

    public static attribute @checked = values
        => attribute<@checked>(values);

    public static attribute cite = values
        => attribute<cite>(values);

    public static attribute color = values
        => attribute<color>(values);

    public static attribute cols = values
        => attribute<cols>(values);

    public static attribute colspan = values
        => attribute<colspan>(values);

    public static attribute content = values
        => attribute<content>(values);

    public static attribute contenteditable = values
        => attribute<contenteditable>(values);

    public static attribute controls = values
        => attribute<controls>(values);

    public static attribute coords = values
        => attribute<coords>(values);

    public static attribute crossorigin = values
        => attribute<crossorigin>(values);

    public static attribute data = values
        => attribute<data>(values);

    public static attribute datetime = values
        => attribute<datetime>(values);

    public static attribute decoding = values
        => attribute<decoding>(values);

    public static attribute @default = values
        => attribute<@default>(values);

    public static attribute defer = values
        => attribute<defer>(values);

    public static attribute dir = values
        => attribute<dir>(values);

    public static attribute dirname = values
        => attribute<dirname>(values);

    public static attribute disabled = values
        => attribute<disabled>(values);

    public static attribute download = values
        => attribute<download>(values);

    public static attribute draggable = values
        => attribute<draggable>(values);

    public static attribute enctype = values
        => attribute<enctype>(values);

    public static attribute @for = values
        => attribute<@for>(values);

    public static attribute form = values
        => attribute<form>(values);

    public static attribute formaction = values
        => attribute<formaction>(values);

    public static attribute headers = values
        => attribute<headers>(values);

    public static attribute height = values
        => attribute<height>(values);

    public static attribute hidden = values
        => attribute<hidden>(values);

    public static attribute high = values
        => attribute<high>(values);

    public static attribute href = values
        => attribute<href>(values);

    public static attribute hreflang = values
        => attribute<hreflang>(values);

    public static attribute httpequiv = values
        => attribute<httpEquiv>(values);


    public static attribute id = values
        => attribute<id>(values);

    public static attribute ismap = values
        => attribute<ismap>(values);

    public static attribute itemprop = values
        => attribute<itemprop>(values);

    public static attribute kind = values
        => attribute<kind>(values);

    public static attribute label = values
        => attribute<label>(values);

    public static attribute lang = values
        => attribute<lang>(values);

    public static attribute list = values
        => attribute<list>(values);

    public static attribute loop = values
        => attribute<loop>(values);

    public static attribute low = values
        => attribute<low>(values);

    public static attribute max = values
        => attribute<max>(values);

    public static attribute maxlength = values
        => attribute<maxlength>(values);

    public static attribute media = values
        => attribute<media>(values);

    public static attribute method = values
        => attribute<method>(values);

    public static attribute min = values
        => attribute<min>(values);

    public static attribute minlength = values
        => attribute<minlength>(values);

    public static attribute multiple = values
        => attribute<multiple>(values);

    public static attribute muted = values
        => attribute<muted>(values);

    public static attribute name = values
        => attribute<name>(values);

    public static attribute novalidate = values
        => attribute<novalidate>(values);

    public static attribute open = values
        => attribute<open>(values);

    public static attribute optimum = values
        => attribute<optimum>(values);

    public static attribute pattern = values
        => attribute<pattern>(values);

    public static attribute ping = values
        => attribute<ping>(values);

    public static attribute placeholder = values
        => attribute<placeholder>(values);

    public static attribute poster = values
        => attribute<poster>(values);

    public static attribute preload = values
        => attribute<preload>(values);

    public static attribute @readonly = values
        => attribute<@readonly>(values);

    public static attribute rel = values
        => attribute<rel>(values);

    public static attribute required = values
        => attribute<required>(values);

    public static attribute reversed = values
        => attribute<reversed>(values);

    public static attribute rows = values
        => attribute<rows>(values);

    public static attribute rowspan = values
        => attribute<rowspan>(values);

    public static attribute sandbox = values
        => attribute<sandbox>(values);

    public static attribute scope = values
        => attribute<scope>(values);

    public static attribute shape = values
        => attribute<shape>(values);

    public static attribute size = values
        => attribute<size>(values);

    public static attribute sizes = values
        => attribute<sizes>(values);

    public static attribute slot = values
        => attribute<slot>(values);

    public static attribute span = values
        => attribute<span>(values);

    public static attribute spellcheck = values
        => attribute<spellcheck>(values);

    public static attribute src = values
        => attribute<src>(values);

    public static attribute srcdoc = values
        => attribute<srcdoc>(values);

    public static attribute srclang = values
        => attribute<srclang>(values);

    public static attribute srcset = values
        => attribute<srcset>(values);

    public static attribute start = values
        => attribute<start>(values);

    public static attribute step = values
        => attribute<step>(values);

    public static attribute tabindex = values
        => attribute<tabindex>(values);

    public static attribute target = values
        => attribute<target>(values);

    public static attribute title = values
        => attribute<title>(values);

    public static attribute translate = values
        => attribute<translate>(values);

    public static attribute type = values
        => attribute<type>(values);

    public static attribute usemap = values
        => attribute<usemap>(values);

    public static attribute value = values
        => attribute<value>(values);

    public static attribute width = values
        => attribute<width>(values);

    public static attribute wrap = values
        => attribute<wrap>(values);

    public static Attribute<T> attribute<T>(params string[] values)
        where T : Literal<T>, AttributeName =>
        new(values);

    public static Data.Attribute attribute(string name, params string[] values) =>
        new(name, values);
}
