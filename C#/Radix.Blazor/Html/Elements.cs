using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public delegate Node element(IEnumerable<Attribute> attributes, IEnumerable<Node> children);

    public static class Elements
    {
        public static Text Text(string text)
            => new Text(text);

        public static Empty Empty
            => new Empty();

        public static Element Element(Name name, IEnumerable<Attribute> attributes, IEnumerable<Node> children)
        => new Element(name, attributes, children);

        public static element a = (attributes, children)
            => Element(new Name("a"), attributes, children);

        public static element abbr = (attributes, children)
            => Element(new Name("abbr"), attributes, children);

        public static element acronym = (attributes, children)
            => Element(new Name("acronym"), attributes, children);

        public static element address = (attributes, children)
            => Element(new Name("address"), attributes, children);

        public static element applet = (attributes, children)
            => Element(new Name("applet"), attributes, children);

        public static element area = (attributes, children)
            => Element(new Name("area"), attributes, children);

        public static element article = (attributes, children)
            => Element(new Name("article"), attributes, children);

        public static element aside = (attributes, children)
            => Element(new Name("aside"), attributes, children);

        public static element audio = (attributes, children)
            => Element(new Name("audio"), attributes, children);

        public static element b = (attributes, children)
            => Element(new Name("b"), attributes, children);

        public static element @base = (attributes, children)
            => Element(new Name("base"), attributes, children);

        public static element baseFont = (attributes, children)
            => Element(new Name("basefont"), attributes, children);

        public static element bdi = (attributes, children)
            => Element(new Name("bdi"), attributes, children);

        public static element bdo = (attributes, children)
            => Element(new Name("bdo"), attributes, children);

        public static element big = (attributes, children)
            => Element(new Name("big"), attributes, children);

        public static element button = (attributes, children)
            => Element(new Name("button"), attributes, children);

        public static element canvas = (attributes, children)
            => Element(new Name("canvas"), attributes, children);

        public static element caption = (attributes, children)
            => Element(new Name("caption"), attributes, children);

        public static element center = (attributes, children)
            => Element(new Name("center"), attributes, children);

        public static element cite = (attributes, children)
            => Element(new Name("cite"), attributes, children);

        public static element code = (attributes, children)
            => Element(new Name("code"), attributes, children);

        public static element col = (attributes, children)
            => Element(new Name("col"), attributes, children);

        public static element colgroup = (attributes, children)
            => Element(new Name("colgroup"), attributes, children);

        public static element content = (attributes, children)
            => Element(new Name("content"), attributes, children);

        public static element data = (attributes, children)
            => Element(new Name("data"), attributes, children);

        public static element datalist = (attributes, children)
            => Element(new Name("datalist"), attributes, children);

        public static element dd = (attributes, children)
            => Element(new Name("dd"), attributes, children);

        public static element del = (attributes, children)
            => Element(new Name("del"), attributes, children);

        public static element details = (attributes, children)
            => Element(new Name("details"), attributes, children);

        public static element dfn = (attributes, children)
            => Element(new Name("dfn"), attributes, children);

        public static element dialog = (attributes, children)
            => Element(new Name("dialog"), attributes, children);

        public static element dir = (attributes, children)
            => Element(new Name("dir"), attributes, children);

        public static element div = (attributes, children)
            => Element(new Name("div"), attributes, children);

        public static element dl = (attributes, children)
            => Element(new Name("dl"), attributes, children);

        public static element element = (attributes, children)
            => Element(new Name("element"), attributes, children);

        public static element dt = (attributes, children)
            => Element(new Name("dt"), attributes, children);

        public static element em = (attributes, children)
            => Element(new Name("em"), attributes, children);

        public static element embed = (attributes, children)
            => Element(new Name("embed"), attributes, children);

        public static element fieldset = (attributes, children)
            => Element(new Name("fieldset"), attributes, children);

        public static element figcaption = (attributes, children)
            => Element(new Name("figcaption"), attributes, children);

        public static element figure = (attributes, children)
            => Element(new Name("figure"), attributes, children);

        public static element font = (attributes, children)
            => Element(new Name("font"), attributes, children);

        public static element footer = (attributes, children)
            => Element(new Name("footer"), attributes, children);

        public static element form = (attributes, children)
            => Element(new Name("form"), attributes, children);

        public static element frame = (attributes, children)
            => Element(new Name("frame"), attributes, children);

        public static element frameset = (attributes, children)
            => Element(new Name("frameset"), attributes, children);

        public static element h1 = (attributes, children)
            => Element(new Name("h1"), attributes, children);

        public static element h2 = (attributes, children)
            => Element(new Name("h2"), attributes, children);

        public static element h3 = (attributes, children)
            => Element(new Name("h3"), attributes, children);

        public static element h4 = (attributes, children)
            => Element(new Name("h4"), attributes, children);

        public static element h5 = (attributes, children)
            => Element(new Name("h5"), attributes, children);

        public static element h6 = (attributes, children)
            => Element(new Name("h6"), attributes, children);

        public static element head = (attributes, children)
            => Element(new Name("head"), attributes, children);

        public static element header = (attributes, children)
            => Element(new Name("header"), attributes, children);

        public static element hr = (attributes, children)
            => Element(new Name("hr"), attributes, children);

        public static element html = (attributes, children)
            => Element(new Name("html"), attributes, children);

        public static element i = (attributes, children)
            => Element(new Name("i"), attributes, children);

        public static element iframe = (attributes, children)
            => Element(new Name("iframe"), attributes, children);

        public static element img = (attributes, children)
            => Element(new Name("img"), attributes, children);

        public static element input = (attributes, children)
            => Element(new Name("input"), attributes, children);

        public static element ins = (attributes, children)
            => Element(new Name("ins"), attributes, children);

        public static element kbd = (attributes, children)
            => Element(new Name("kbd"), attributes, children);

        public static element label = (attributes, children)
            => Element(new Name("label"), attributes, children);

        public static element legend = (attributes, children)
            => Element(new Name("legend"), attributes, children);

        public static element li = (attributes, children)
            => Element(new Name("li"), attributes, children);

        public static element link = (attributes, children)
            => Element(new Name("link"), attributes, children);

        public static element main = (attributes, children)
            => Element(new Name("main"), attributes, children);

        public static element map = (attributes, children)
            => Element(new Name("map"), attributes, children);

        public static element mark = (attributes, children)
            => Element(new Name("mark"), attributes, children);

        public static element menu = (attributes, children)
            => Element(new Name("menu"), attributes, children);

        public static element menuitem = (attributes, children)
            => Element(new Name("menuitem"), attributes, children);

        public static element meta = (attributes, children)
            => Element(new Name("meta"), attributes, children);

        public static element meter = (attributes, children)
            => Element(new Name("meter"), attributes, children);

        public static element nav = (attributes, children)
            => Element(new Name("nav"), attributes, children);

        public static element noembed = (attributes, children)
            => Element(new Name("noembed"), attributes, children);

        public static element noframes = (attributes, children)
            => Element(new Name("noframes"), attributes, children);

        public static element noscript = (attributes, children)
            => Element(new Name("noscript"), attributes, children);

        public static element Object = (attributes, children)
            => Element(new Name("object"), attributes, children);

        public static element ol = (attributes, children)
            => Element(new Name("ol"), attributes, children);

        public static element optgroup = (attributes, children)
            => Element(new Name("optgroup"), attributes, children);

        public static element option = (attributes, children)
            => Element(new Name("option"), attributes, children);

        public static element output = (attributes, children)
            => Element(new Name("output"), attributes, children);

        public static element p = (attributes, children)
            => Element(new Name("p"), attributes, children);

        public static element param = (attributes, children)
            => Element(new Name("param"), attributes, children);

        public static element picture = (attributes, children)
            => Element(new Name("picture"), attributes, children);

        public static element pre = (attributes, children)
            => Element(new Name("pre"), attributes, children);

        public static element progress = (attributes, children)
            => Element(new Name("progress"), attributes, children);

        public static element q = (attributes, children)
            => Element(new Name("q"), attributes, children);

        public static element rb = (attributes, children)
            => Element(new Name("rb"), attributes, children);

        public static element rp = (attributes, children)
            => Element(new Name("rp"), attributes, children);

        public static element rt = (attributes, children)
            => Element(new Name("rt"), attributes, children);

        public static element rtc = (attributes, children)
            => Element(new Name("rtc"), attributes, children);

        public static element ruby = (attributes, children)
            => Element(new Name("ruby"), attributes, children);

        public static element s = (attributes, children)
            => Element(new Name("s"), attributes, children);

        public static element samp = (attributes, children)
            => Element(new Name("samp"), attributes, children);

        public static element script = (attributes, children)
            => Element(new Name("script"), attributes, children);

        public static element section = (attributes, children)
            => Element(new Name("section"), attributes, children);

        public static element select = (attributes, children)
            => Element(new Name("select"), attributes, children);

        public static element shadow = (attributes, children)
            => Element(new Name("shadow"), attributes, children);

        public static element slot = (attributes, children)
            => Element(new Name("slot"), attributes, children);

        public static element small = (attributes, children)
            => Element(new Name("small"), attributes, children);

        //public static node source = (attributes) 
        //    => Element(new Name("source"), attributes, Enumerable.Empty);

        public static element span = (attributes, children)
            => Element(new Name("span"), attributes, children);

        public static element strike = (attributes, children)
            => Element(new Name("strike"), attributes, children);

        public static element strong = (attributes, children)
            => Element(new Name("strong"), attributes, children);

        public static element style = (attributes, children)
            => Element(new Name("style"), attributes, children);

        public static element sub = (attributes, children)
            => Element(new Name("sub"), attributes, children);

        public static element summary = (attributes, children)
            => Element(new Name("summary"), attributes, children);

        public static element sup = (attributes, children)
            => Element(new Name("sup"), attributes, children);

        public static element svg = (attributes, children)
            => Element(new Name("svg"), attributes, children);

        public static element table = (attributes, children)
            => Element(new Name("table"), attributes, children);

        public static element tbody = (attributes, children)
            => Element(new Name("tbody"), attributes, children);

        public static element td = (attributes, children)
            => Element(new Name("td"), attributes, children);

        public static element template = (attributes, children)
            => Element(new Name("template"), attributes, children);

        public static element textarea = (attributes, children)
            => Element(new Name("textarea"), attributes, children);

        public static element tfoot = (attributes, children)
            => Element(new Name("tfoot"), attributes, children);

        public static element th = (attributes, children)
            => Element(new Name("th"), attributes, children);

        public static element thead = (attributes, children)
            => Element(new Name("thead"), attributes, children);

        public static element time = (attributes, children)
            => Element(new Name("time"), attributes, children);

        public static element title = (attributes, children)
            => Element(new Name("title"), attributes, children);

        public static element tr = (attributes, children)
            => Element(new Name("tr"), attributes, children);

        public static element track = (attributes, children)
            => Element(new Name("track"), attributes, children);

        public static element tt = (attributes, children)
            => Element(new Name("tt"), attributes, children);

        public static element u = (attributes, children)
            => Element(new Name("u"), attributes, children);

        public static element ul = (attributes, children)
            => Element(new Name("ul"), attributes, children);

        public static element var = (attributes, children)
            => Element(new Name("var"), attributes, children);

        public static element video = (attributes, children)
            => Element(new Name("video"), attributes, children);

        public static element wbr = (attributes, children)
            => Element(new Name("wbr"), attributes, children);

    }
}