using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Blazor.Html
{
    public delegate Attribute attribute(IEnumerable<NonNullString> values);

    public static class Attributes
    {
        public static Attribute attribute(Name name, IEnumerable<NonNullString> values)
            => new Attribute(name, values); 

        public static attribute accept = (values)
            => attribute(new Name("accept"), values);

        public static attribute acceptCharset = (values)
            => attribute(new Name("acceptCharset"), values);

        public static attribute accesskey = (values)
            => attribute(new Name("accesskey"), values);

        public static attribute action = (values)
            => attribute(new Name("action"), values);

        public static attribute align = (values)
            => attribute(new Name("align"), values);

        public static attribute allow = (values)
            => attribute(new Name("allow"), values);

        public static attribute alt = (values)
            => attribute(new Name("alt"), values);

        public static attribute async = (values)
            => attribute(new Name("async"), values);

        public static attribute autocapitalize = (values)
            => attribute(new Name("autocapitalize"), values);

        public static attribute autocomplete = (values)
            => attribute(new Name("autocomplete"), values);

        public static attribute autofocus = (values)
            => attribute(new Name("autofocus"), values);

        public static attribute autoplay = (values)
            => attribute(new Name("autoplay"), values);

        public static attribute bgcolor = (values)
            => attribute(new Name("bgcolor"), values);

        public static attribute border = (values)
            => attribute(new Name("border"), values);

        public static attribute buffered = (values)
            => attribute(new Name("buffered"), values);

        public static attribute challenge = (values)
            => attribute(new Name("challenge"), values);

        public static attribute charset = (values)
            => attribute(new Name("charset"), values);

        public static attribute @checked = (values)
            => attribute(new Name("checked"), values);

        public static attribute cite = (values)
            => attribute(new Name("cite"), values);

        public static attribute code = (values)
            => attribute(new Name("code"), values);

        public static attribute codebase = (values)
            => attribute(new Name("codebase"), values);

        public static attribute color = (values)
            => attribute(new Name("color"), values);

        public static attribute cols = (values)
            => attribute(new Name("cols"), values);

        public static attribute colspan = (values)
            => attribute(new Name("colspan"), values);
        
        public static attribute colspan = (values)
            => attribute(new Name("colspan"), values);
        
        public static attribute content = (values)
            => attribute(new Name("content"), values);
        
        public static attribute contenteditable = (values)
            => attribute(new Name("contenteditable"), values);
        
        public static attribute contextmenu = (values)
            => attribute(new Name("contextmenu"), values);
        
        public static attribute controls = (values)
            => attribute(new Name("controls"), values);
        
        public static attribute coords = (values)
            => attribute(new Name("coords"), values);
        
        public static attribute crossorigin = (values)
            => attribute(new Name("crossorigin"), values);
        
        public static attribute csp = (values)
            => attribute(new Name("csp"), values);
        
        public static attribute data = (values)
            => attribute(new Name("data"), values);
        
        public static attribute datetime = (values)
            => attribute(new Name("datetime"), values);
        
        public static attribute decoding = (values)
            => attribute(new Name("decoding"), values);
        
        public static attribute @default = (values)
            => attribute(new Name("default"), values);
        
        public static attribute defer = (values)
            => attribute(new Name("defer"), values);
        
        public static attribute dir = (values)
            => attribute(new Name("dir"), values);
        
        public static attribute dirname = (values)
            => attribute(new Name("dirname"), values);
        
        public static attribute disabled = (values)
            => attribute(new Name("disabled"), values);
        
        public static attribute download = (values)
            => attribute(new Name("download"), values);
        
        public static attribute draggable = (values)
            => attribute(new Name("draggable"), values);
        
        public static attribute dropzone = (values)
            => attribute(new Name("dropzone"), values);
        
        public static attribute enctype = (values)
            => attribute(new Name("enctype"), values);
        
        public static attribute @for = (values)
            => attribute(new Name("for"), values);
        
        public static attribute form = (values)
            => attribute(new Name("form"), values);
        
        public static attribute formaction = (values)
            => attribute(new Name("formaction"), values);
        
        public static attribute headers = (values)
            => attribute(new Name("headers"), values);
        
        public static attribute height = (values)
            => attribute(new Name("height"), values);
        
        public static attribute hidden = (values)
            => attribute(new Name("hidden"), values);
        
        public static attribute high = (values)
            => attribute(new Name("high"), values);
        
        public static attribute href = (values)
            => attribute(new Name("href"), values);
        
        public static attribute hreflang = (values)
            => attribute(new Name("hreflang"), values);
        
        public static attribute httpequiv = (values)
            => attribute(new Name("http-equiv"), values);
        
        public static attribute icon = (values)
            => attribute(new Name("icon"), values);
        
        public static attribute id = (values)
            => attribute(new Name("id"), values);
        
        public static attribute importance = (values)
            => attribute(new Name("importance"), values);

        public static attribute integrity = (values)
            => attribute(new Name("integrity"), values);

        public static attribute ismap = (values)
            => attribute(new Name("ismap"), values);

        public static attribute itemprop = (values)
            => attribute(new Name("itemprop"), values);

        public static attribute keytype = (values)
            => attribute(new Name("keytype"), values);

        public static attribute kind = (values)
            => attribute(new Name("kind"), values);

        public static attribute label = (values)
            => attribute(new Name("label"), values);

        public static attribute lang = (values)
            => attribute(new Name("lang"), values);

        public static attribute language = (values)
            => attribute(new Name("language"), values);

        public static attribute lazyload = (values)
            => attribute(new Name("lazyload"), values);

        public static attribute list = (values)
            => attribute(new Name("list"), values);

        public static attribute loop = (values)
            => attribute(new Name("loop"), values);

        public static attribute low = (values)
            => attribute(new Name("low"), values);

        public static attribute manifest = (values)
            => attribute(new Name("manifest"), values);

        public static attribute max = (values)
            => attribute(new Name("max"), values);

        public static attribute maxlength = (values)
            => attribute(new Name("maxlength"), values);

        public static attribute media = (values)
            => attribute(new Name("media"), values);

        public static attribute method = (values)
            => attribute(new Name("method"), values);

        public static attribute min = (values)
            => attribute(new Name("min"), values);

        public static attribute minlength = (values)
            => attribute(new Name("minlength"), values);

        public static attribute multiple = (values)
            => attribute(new Name("multiple"), values);

        public static attribute muted = (values)
            => attribute(new Name("muted"), values);

        public static attribute name = (values)
            => attribute(new Name("name"), values);

        public static attribute novalidate = (values)
            => attribute(new Name("novalidate"), values);

        public static attribute open = (values)
            => attribute(new Name("open"), values);

        public static attribute optimum = (values)
            => attribute(new Name("optimum"), values);

        public static attribute pattern = (values)
            => attribute(new Name("pattern"), values);

        public static attribute ping = (values)
            => attribute(new Name("ping"), values);

        public static attribute placeholder = (values)
            => attribute(new Name("placeholder"), values);

        public static attribute poster = (values)
            => attribute(new Name("poster"), values);

        public static attribute preload = (values)
            => attribute(new Name("preload"), values);

        public static attribute @readonly = (values)
            => attribute(new Name("readonly"), values);

        public static attribute rel = (values)
            => attribute(new Name("rel"), values);
        
        public static attribute required = (values)
            => attribute(new Name("required"), values);
        
        public static attribute reversed = (values)
            => attribute(new Name("reversed"), values);
        
        public static attribute rows = (values)
            => attribute(new Name("rows"), values);
        
        public static attribute rowspan = (values)
            => attribute(new Name("rowspan"), values);
        
        public static attribute sandbox = (values)
            => attribute(new Name("sandbox"), values);
        
        public static attribute scope = (values)
            => attribute(new Name("scope"), values);
        
        public static attribute selected = (values)
            => attribute(new Name("selected"), values);
        
        public static attribute shape = (values)
            => attribute(new Name("shape"), values);
        
        public static attribute size = (values)
            => attribute(new Name("size"), values);
        
        public static attribute sizes = (values)
            => attribute(new Name("sizes"), values);
        
        public static attribute slot = (values)
            => attribute(new Name("slot"), values);
        
        public static attribute span = (values)
            => attribute(new Name("span"), values);
        
        public static attribute spellcheck = (values)
            => attribute(new Name("spellcheck"), values);
        
        public static attribute src = (values)
            => attribute(new Name("src"), values);
        
        public static attribute srcdoc = (values)
            => attribute(new Name("srcdoc"), values);
        
        public static attribute srclang = (values)
            => attribute(new Name("srclang"), values);
        
        public static attribute srcset = (values)
            => attribute(new Name("srcset"), values);
        
        public static attribute start = (values)
            => attribute(new Name("start"), values);
        
        public static attribute step = (values)
            => attribute(new Name("step"), values);
        
        public static attribute style = (values)
            => attribute(new Name("style"), values);
        
        public static attribute summary = (values)
            => attribute(new Name("summary"), values);
        
        public static attribute tabindex = (values)
            => attribute(new Name("tabindex"), values);
        
        public static attribute target = (values)
            => attribute(new Name("target"), values);
        
        public static attribute title = (values)
            => attribute(new Name("title"), values);
        
        public static attribute translate = (values)
            => attribute(new Name("translate"), values);
        
        public static attribute type = (values)
            => attribute(new Name("type"), values);
        
        public static attribute usemap = (values)
            => attribute(new Name("usemap"), values);
        
        public static attribute value = (values)
            => attribute(new Name("value"), values);
        
        public static attribute width = (values)
            => attribute(new Name("width"), values);
        
        public static attribute wrap = (values)
            => attribute(new Name("wrap"), values);
    }
}
