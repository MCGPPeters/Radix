namespace Radix.Components.Html;

public delegate IAttribute attribute(params string[] values);

public static class Attributes
{
    public static IEnumerable<IAttribute> None => Enumerable.Empty<IAttribute>();

    public static attribute accept = values
        => attribute(nameof(accept), values);

    public static attribute acceptCharset = values
        => attribute(nameof(acceptCharset), values);

    public static attribute accesskey = values
        => attribute(nameof(accesskey), values);

    public static attribute action = values
        => attribute(nameof(action), values);

    public static attribute align = values
        => attribute(nameof(align), values);

    public static attribute allow = values
        => attribute(nameof(allow), values);

    public static attribute alt = values
        => attribute(nameof(alt), values);

    public static attribute aria_label = values
        => attribute("aria-label", values);

    public static attribute async = values
        => attribute(nameof(async), values);

    public static attribute autocapitalize = values
        => attribute(nameof(autocapitalize), values);

    public static attribute autocomplete = values
        => attribute(nameof(autocomplete), values);

    public static attribute autofocus = values
        => attribute(nameof(autofocus), values);

    public static attribute autoplay = values
        => attribute(nameof(autoplay), values);

    public static attribute bgcolor = values
        => attribute(nameof(bgcolor), values);

    public static attribute border = values
        => attribute(nameof(border), values);

    public static attribute buffered = values
        => attribute(nameof(buffered), values);

    public static attribute challenge = values
        => attribute(nameof(challenge), values);

    public static attribute @class = values
        => attribute(nameof(@class), values);

    public static attribute charset = values
        => attribute(nameof(charset), values);

    public static attribute @checked = values
        => attribute(nameof(@checked), values);

    public static attribute cite = values
        => attribute(nameof(cite), values);

    public static attribute code = values
        => attribute(nameof(code), values);

    public static attribute codebase = values
        => attribute(nameof(codebase), values);

    public static attribute color = values
        => attribute(nameof(color), values);

    public static attribute cols = values
        => attribute(nameof(cols), values);

    public static attribute colspan = values
        => attribute(nameof(colspan), values);

    public static attribute content = values
        => attribute(nameof(content), values);

    public static attribute contenteditable = values
        => attribute(nameof(contenteditable), values);

    public static attribute contextmenu = values
        => attribute(nameof(contextmenu), values);

    public static attribute controls = values
        => attribute(nameof(controls), values);

    public static attribute coords = values
        => attribute(nameof(coords), values);

    public static attribute crossorigin = values
        => attribute(nameof(crossorigin), values);

    public static attribute csp = values
        => attribute(nameof(csp), values);

    public static attribute data = values
        => attribute(nameof(data), values);

    public static attribute datetime = values
        => attribute(nameof(datetime), values);

    public static attribute decoding = values
        => attribute(nameof(decoding), values);

    public static attribute @default = values
        => attribute(nameof(@default), values);

    public static attribute defer = values
        => attribute(nameof(defer), values);

    public static attribute dir = values
        => attribute(nameof(dir), values);

    public static attribute dirname = values
        => attribute(nameof(dirname), values);

    public static attribute disabled = values
        => attribute(nameof(disabled), values);

    public static attribute download = values
        => attribute(nameof(download), values);

    public static attribute draggable = values
        => attribute(nameof(draggable), values);

    public static attribute dropzone = values
        => attribute(nameof(dropzone), values);

    public static attribute enctype = values
        => attribute(nameof(enctype), values);

    public static attribute @for = values
        => attribute(nameof(@for), values);

    public static attribute form = values
        => attribute(nameof(form), values);

    public static attribute formaction = values
        => attribute(nameof(formaction), values);

    public static attribute headers = values
        => attribute(nameof(headers), values);

    public static attribute height = values
        => attribute(nameof(height), values);

    public static attribute hidden = values
        => attribute(nameof(hidden), values);

    public static attribute high = values
        => attribute(nameof(high), values);

    public static attribute href = values
        => attribute(nameof(href), values);

    public static attribute hreflang = values
        => attribute(nameof(hreflang), values);

    public static attribute httpequiv = values
        => attribute("http-equiv", values);

    public static attribute icon = values
        => attribute(nameof(icon), values);

    public static attribute id = values
        => attribute(nameof(id), values);

    public static attribute importance = values
        => attribute(nameof(importance), values);

    public static attribute integrity = values
        => attribute(nameof(integrity), values);

    public static attribute ismap = values
        => attribute(nameof(ismap), values);

    public static attribute itemprop = values
        => attribute(nameof(itemprop), values);

    public static attribute keytype = values
        => attribute(nameof(keytype), values);

    public static attribute kind = values
        => attribute(nameof(kind), values);

    public static attribute label = values
        => attribute(nameof(label), values);

    public static attribute lang = values
        => attribute(nameof(lang), values);

    public static attribute language = values
        => attribute(nameof(language), values);

    public static attribute lazyload = values
        => attribute(nameof(lazyload), values);

    public static attribute list = values
        => attribute(nameof(list), values);

    public static attribute loop = values
        => attribute(nameof(loop), values);

    public static attribute low = values
        => attribute(nameof(low), values);

    public static attribute manifest = values
        => attribute(nameof(manifest), values);

    public static attribute max = values
        => attribute(nameof(max), values);

    public static attribute maxlength = values
        => attribute(nameof(maxlength), values);

    public static attribute media = values
        => attribute(nameof(media), values);

    public static attribute method = values
        => attribute(nameof(method), values);

    public static attribute min = values
        => attribute(nameof(min), values);

    public static attribute minlength = values
        => attribute(nameof(minlength), values);

    public static attribute multiple = values
        => attribute(nameof(multiple), values);

    public static attribute muted = values
        => attribute(nameof(muted), values);

    public static attribute name = values
        => attribute(nameof(name), values);

    public static attribute novalidate = values
        => attribute(nameof(novalidate), values);

    public static attribute open = values
        => attribute(nameof(open), values);

    public static attribute optimum = values
        => attribute(nameof(optimum), values);

    public static attribute pattern = values
        => attribute(nameof(pattern), values);

    public static attribute ping = values
        => attribute(nameof(ping), values);

    public static attribute placeholder = values
        => attribute(nameof(placeholder), values);

    public static attribute poster = values
        => attribute(nameof(poster), values);

    public static attribute preload = values
        => attribute(nameof(preload), values);

    public static attribute @readonly = values
        => attribute(nameof(@readonly), values);

    public static attribute rel = values
        => attribute(nameof(rel), values);

    public static attribute required = values
        => attribute(nameof(required), values);

    public static attribute reversed = values
        => attribute(nameof(reversed), values);

    public static attribute rows = values
        => attribute(nameof(rows), values);

    public static attribute rowspan = values
        => attribute(nameof(rowspan), values);

    public static attribute sandbox = values
        => attribute(nameof(sandbox), values);

    public static attribute scope = values
        => attribute(nameof(scope), values);

    public static attribute selected = values
        => attribute(nameof(selected), values);

    public static attribute shape = values
        => attribute(nameof(shape), values);

    public static attribute size = values
        => attribute(nameof(size), values);

    public static attribute sizes = values
        => attribute(nameof(sizes), values);

    public static attribute slot = values
        => attribute(nameof(slot), values);

    public static attribute span = values
        => attribute(nameof(span), values);

    public static attribute spellcheck = values
        => attribute(nameof(spellcheck), values);

    public static attribute src = values
        => attribute(nameof(src), values);

    public static attribute srcdoc = values
        => attribute(nameof(srcdoc), values);

    public static attribute srclang = values
        => attribute(nameof(srclang), values);

    public static attribute srcset = values
        => attribute(nameof(srcset), values);

    public static attribute start = values
        => attribute(nameof(start), values);

    public static attribute step = values
        => attribute(nameof(step), values);

    public static attribute style = values
        => attribute(nameof(style), values);

    public static attribute summary = values
        => attribute(nameof(summary), values);

    public static attribute tabindex = values
        => attribute(nameof(tabindex), values);

    public static attribute target = values
        => attribute(nameof(target), values);

    public static attribute title = values
        => attribute(nameof(title), values);

    public static attribute translate = values
        => attribute(nameof(translate), values);

    public static attribute type = values
        => attribute(nameof(type), values);

    public static attribute usemap = values
        => attribute(nameof(usemap), values);

    public static attribute value = values
        => attribute(nameof(value), values);

    public static attribute width = values
        => attribute(nameof(width), values);

    public static attribute wrap = values
        => attribute(nameof(wrap), values);

    public static Attribute attribute(string name, params string[] values) => new(name, values);
}
