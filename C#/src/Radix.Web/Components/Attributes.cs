using Radix.Web.Html.Data.Names.Attributes;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Web.Components;

public delegate Data.Attribute<string> attribute(AttributeId attributeId, params string[] values);

public static class Attributes
{
    public static attribute accept => (id, values) 
        => attribute<accept>(id, values);

    public static attribute acceptCharset => (id, values) 
        => attribute<acceptCharset>(id, values);

    public static attribute accesskey => (id, values) 
        => attribute<accesskey>(id, values);

    public static attribute action => (id, values) 
        => attribute<action>(id, values);

    public static attribute allow => (id, values) 
        => attribute<allow>(id, values);

    public static attribute alt => (id, values) 
        => attribute<alt>(id, values);

    public static attribute aria_label => (id, values) 
        => attribute<ariaLabel>(id, values);

    public static attribute async => (id, values) 
        => attribute<async>(id, values);

    public static attribute autocapitalize => (id, values) 
        => attribute<autocapitalize>(id, values);

    public static attribute autocomplete => (id, values) 
        => attribute<autocomplete>(id, values);

    public static attribute autofocus => (id, values) 
        => attribute<autofocus>(id, values);

    public static attribute autoplay => (id, values) 
        => attribute<autoplay>(id, values);

    public static attribute @class => (id, values) 
        => attribute<@class>(id, values);

    public static attribute charset => (id, values) 
        => attribute<charset>(id, values);

    public static attribute @checked => (id, values) 
        => attribute<@checked>(id, values);

    public static attribute cite => (id, values) 
        => attribute<cite>(id, values);

    public static attribute color => (id, values) 
        => attribute<color>(id, values);

    public static attribute cols => (id, values) 
        => attribute<cols>(id, values);

    public static attribute colspan => (id, values) 
        => attribute<colspan>(id, values);

    public static attribute content => (id, values) 
        => attribute<content>(id, values);

    public static attribute contenteditable => (id, values) 
        => attribute<contenteditable>(id, values);

    public static attribute controls => (id, values) 
        => attribute<controls>(id, values);

    public static attribute coords => (id, values) 
        => attribute<coords>(id, values);

    public static attribute crossorigin => (id, values) 
        => attribute<crossorigin>(id, values);

    public static attribute data => (id, values) 
        => attribute<data>(id, values);

    public static attribute datetime => (id, values) 
        => attribute<datetime>(id, values);

    public static attribute decoding => (id, values) 
        => attribute<decoding>(id, values);

    public static attribute @default => (id, values) 
        => attribute<@default>(id, values);

    public static attribute defer => (id, values) 
        => attribute<defer>(id, values);

    public static attribute dir => (id, values) 
        => attribute<dir>(id, values);

    public static attribute dirname => (id, values) 
        => attribute<dirname>(id, values);

    public static attribute disabled => (id, values) 
        => attribute<disabled>(id, values);

    public static attribute download => (id, values) 
        => attribute<download>(id, values);

    public static attribute draggable => (id, values) 
        => attribute<draggable>(id, values);

    public static attribute enctype => (id, values) 
        => attribute<enctype>(id, values);

    public static attribute @for => (id, values) 
        => attribute<@for>(id, values);

    public static attribute form => (id, values) 
        => attribute<form>(id, values);

    public static attribute formaction => (id, values) 
        => attribute<formaction>(id, values);

    public static attribute headers => (id, values) 
        => attribute<headers>(id, values);

    public static attribute height => (id, values) 
        => attribute<height>(id, values);

    public static attribute hidden => (id, values) 
        => attribute<hidden>(id, values);

    public static attribute high => (id, values) 
        => attribute<high>(id, values);

    public static attribute href => (id, values) 
        => attribute<href>(id, values);

    public static attribute hreflang => (id, values) 
        => attribute<hreflang>(id, values);

    public static attribute httpequiv => (id, values) 
        => attribute<httpEquiv>(id, values);


    public static attribute id => (id, values) 
        => attribute<id>(id, values);

    public static attribute ismap => (id, values) 
        => attribute<ismap>(id, values);

    public static attribute itemprop => (id, values) 
        => attribute<itemprop>(id, values);

    public static attribute kind => (id, values) 
        => attribute<kind>(id, values);

    public static attribute label => (id, values) 
        => attribute<label>(id, values);

    public static attribute lang => (id, values) 
        => attribute<lang>(id, values);

    public static attribute list => (id, values) 
        => attribute<list>(id, values);

    public static attribute loop => (id, values) 
        => attribute<loop>(id, values);

    public static attribute low => (id, values) 
        => attribute<low>(id, values);

    public static attribute max => (id, values) 
        => attribute<max>(id, values);

    public static attribute maxlength => (id, values) 
        => attribute<maxlength>(id, values);

    public static attribute media => (id, values) 
        => attribute<media>(id, values);

    public static attribute method => (id, values) 
        => attribute<method>(id, values);

    public static attribute min => (id, values) 
        => attribute<min>(id, values);

    public static attribute minlength => (id, values) 
        => attribute<minlength>(id, values);

    public static attribute multiple => (id, values) 
        => attribute<multiple>(id, values);

    public static attribute muted => (id, values) 
        => attribute<muted>(id, values);

    public static attribute name => (id, values) 
        => attribute<name>(id, values);

    public static attribute novalidate => (id, values) 
        => attribute<novalidate>(id, values);

    public static attribute open => (id, values) 
        => attribute<open>(id, values);

    public static attribute optimum => (id, values) 
        => attribute<optimum>(id, values);

    public static attribute pattern => (id, values) 
        => attribute<pattern>(id, values);

    public static attribute ping => (id, values) 
        => attribute<ping>(id, values);

    public static attribute placeholder => (id, values) 
        => attribute<placeholder>(id, values);

    public static attribute poster => (id, values) 
        => attribute<poster>(id, values);

    public static attribute preload => (id, values) 
        => attribute<preload>(id, values);

    public static attribute @readonly => (id, values) 
        => attribute<@readonly>(id, values);

    public static attribute rel => (id, values) 
        => attribute<rel>(id, values);

    public static attribute required => (id, values) 
        => attribute<required>(id, values);

    public static attribute reversed => (id, values) 
        => attribute<reversed>(id, values);

    public static attribute rows => (id, values) 
        => attribute<rows>(id, values);

    public static attribute rowspan => (id, values) 
        => attribute<rowspan>(id, values);

    public static attribute sandbox => (id, values) 
        => attribute<sandbox>(id, values);

    public static attribute scope => (id, values) 
        => attribute<scope>(id, values);

    public static attribute shape => (id, values) 
        => attribute<shape>(id, values);

    public static attribute size => (id, values) 
        => attribute<size>(id, values);

    public static attribute sizes => (id, values) 
        => attribute<sizes>(id, values);

    public static attribute slot => (id, values) 
        => attribute<slot>(id, values);

    public static attribute span => (id, values) 
        => attribute<span>(id, values);

    public static attribute spellcheck => (id, values) 
        => attribute<spellcheck>(id, values);

    public static attribute src => (id, values) 
        => attribute<src>(id, values);

    public static attribute srcdoc => (id, values) 
        => attribute<srcdoc>(id, values);

    public static attribute srclang => (id, values) 
        => attribute<srclang>(id, values);

    public static attribute srcset => (id, values) 
        => attribute<srcset>(id, values);

    public static attribute start => (id, values) 
        => attribute<start>(id, values);

    public static attribute step => (id, values) 
        => attribute<step>(id, values);

    public static attribute tabindex => (id, values) 
        => attribute<tabindex>(id, values);

    public static attribute target => (id, values) 
        => attribute<target>(id, values);

    public static attribute title => (id, values) 
        => attribute<title>(id, values);

    public static attribute translate => (id, values) 
        => attribute<translate>(id, values);

    public static attribute type => (id, values) 
        => attribute<type>(id, values);

    public static attribute usemap => (id, values) 
        => attribute<usemap>(id, values);

    public static attribute value => (id, values) 
        => attribute<value>(id, values);

    public static attribute width => (id, values) 
        => attribute<width>(id, values);

    public static attribute wrap => (id, values)
        => attribute<wrap>(id, values);

    public static Data.Attribute<string> attribute<T>(AttributeId id, params string[] values)
        where T : AttributeName, Literal<T> =>
        new Attribute<T>(id, values);

    public static Attribute attribute(AttributeId id, string name, params string[] values) =>
        new Data.Attribute<string>(id, name, values);
}
