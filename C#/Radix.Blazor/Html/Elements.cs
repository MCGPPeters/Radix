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

        public static Element element(Name name, IEnumerable<Attribute> attributes, IEnumerable<Node> children)
        => new Element(name, attributes, children);

        public static element a = (attributes, children)
            => element(new Name("a"), attributes, children);

        public static element abbr = (attributes, children)
            => element(new Name("abbr"), attributes, children);

        public static element acronym = (attributes, children)
            => element(new Name("acronym"), attributes, children);

        public static element address = (attributes, children)
            => element(new Name("address"), attributes, children);

        public static element applet = (attributes, children)
            => element(new Name("applet"), attributes, children);

        public static element area = (attributes, children)
            => element(new Name("area"), attributes, children);

        public static element article = (attributes, children)
            => element(new Name("article"), attributes, children);

        public static element aside = (attributes, children)
            => element(new Name("aside"), attributes, children);

        public static element audio = (attributes, children)
            => element(new Name("audio"), attributes, children);

        public static element b = (attributes, children)
            => element(new Name("b"), attributes, children);

        public static element @base = (attributes, children)
            => element(new Name("base"), attributes, children);

        public static element baseFont = (attributes, children)
            => element(new Name("basefont"), attributes, children);

        public static element bdi = (attributes, children)
            => element(new Name("bdi"), attributes, children);

        public static element bdo = (attributes, children)
            => element(new Name("bdo"), attributes, children);

        public static element big = (attributes, children)
            => element(new Name("big"), attributes, children);

        public static element button = (attributes, children)
            => element(new Name("button"), attributes, children);

        public static element canvas = (attributes, children)
            => element(new Name("canvas"), attributes, children);

        public static element caption = (attributes, children)
            => element(new Name("caption"), attributes, children);

        public static element center = (attributes, children)
            => element(new Name("center"), attributes, children);

        public static element cite = (attributes, children)
            => element(new Name("cite"), attributes, children);

        public static element code = (attributes, children)
            => element(new Name("code"), attributes, children);

        public static element col = (attributes, children)
            => element(new Name("col"), attributes, children);

        public static element colgroup = (attributes, children)
            => element(new Name("colgroup"), attributes, children);

        public static element content = (attributes, children)
            => element(new Name("content"), attributes, children);

        public static element data = (attributes, children)
            => element(new Name("data"), attributes, children);

        public static element datalist = (attributes, children)
            => element(new Name("datalist"), attributes, children);

        public static element dd = (attributes, children)
            => element(new Name("dd"), attributes, children);

        public static element del = (attributes, children)
            => element(new Name("del"), attributes, children);

        public static element details = (attributes, children)
            => element(new Name("details"), attributes, children);

        public static element dfn = (attributes, children)
            => element(new Name("dfn"), attributes, children);

        public static element dialog = (attributes, children)
            => element(new Name("dialog"), attributes, children);

        public static element dir = (attributes, children)
            => element(new Name("dir"), attributes, children);

        public static element div = (attributes, children)
            => element(new Name("div"), attributes, children);

        public static element dl = (attributes, children)
            => element(new Name("dl"), attributes, children);

        public static element element = (attributes, children)
            => element(new Name("element"), attributes, children);

        public static element dt = (attributes, children)
            => element(new Name("dt"), attributes, children);

        public static element em = (attributes, children)
            => element(new Name("em"), attributes, children);

        public static element embed = (attributes, children)
            => element(new Name("embed"), attributes, children);

        public static element fieldset = (attributes, children)
            => element(new Name("fieldset"), attributes, children);

        public static element figcaption = (attributes, children)
            => element(new Name("figcaption"), attributes, children);

        public static element figure = (attributes, children)
            => element(new Name("figure"), attributes, children);

        public static element font = (attributes, children)
            => element(new Name("font"), attributes, children);

        public static element footer = (attributes, children)
            => element(new Name("footer"), attributes, children);

        public static element form = (attributes, children)
            => element(new Name("form"), attributes, children);

        public static element frame = (attributes, children)
            => element(new Name("frame"), attributes, children);

        public static element frameset = (attributes, children)
            => element(new Name("frameset"), attributes, children);

        public static element h1 = (attributes, children)
            => element(new Name("h1"), attributes, children);

        public static element h2 = (attributes, children)
            => element(new Name("h2"), attributes, children);

        public static element h3 = (attributes, children)
            => element(new Name("h3"), attributes, children);

        public static element h4 = (attributes, children)
            => element(new Name("h4"), attributes, children);

        public static element h5 = (attributes, children)
            => element(new Name("h5"), attributes, children);

        public static element h6 = (attributes, children)
            => element(new Name("h6"), attributes, children);

        public static element head = (attributes, children)
            => element(new Name("head"), attributes, children);

        public static element header = (attributes, children)
            => element(new Name("header"), attributes, children);

        public static element hr = (attributes, children)
            => element(new Name("hr"), attributes, children);

        public static element html = (attributes, children)
            => element(new Name("html"), attributes, children);

        public static element i = (attributes, children)
            => element(new Name("i"), attributes, children);

        public static element iframe = (attributes, children)
            => element(new Name("iframe"), attributes, children);

        public static element img = (attributes, children)
            => element(new Name("img"), attributes, children);

        public static element input = (attributes, children)
            => element(new Name("input"), attributes, children);

        public static element ins = (attributes, children)
            => element(new Name("ins"), attributes, children);

        public static element kbd = (attributes, children)
            => element(new Name("kbd"), attributes, children);

        public static element label = (attributes, children)
            => element(new Name("label"), attributes, children);

        public static element legend = (attributes, children)
            => element(new Name("legend"), attributes, children);

        public static element li = (attributes, children)
            => element(new Name("li"), attributes, children);

        public static element link = (attributes, children)
            => element(new Name("link"), attributes, children);

        public static element main = (attributes, children)
            => element(new Name("main"), attributes, children);

        public static element map = (attributes, children)
            => element(new Name("map"), attributes, children);

        public static element mark = (attributes, children)
            => element(new Name("mark"), attributes, children);

        public static element menu = (attributes, children)
            => element(new Name("menu"), attributes, children);

        public static element menuitem = (attributes, children)
            => element(new Name("menuitem"), attributes, children);

        public static element meta = (attributes, children)
            => element(new Name("meta"), attributes, children);

        public static element meter = (attributes, children)
            => element(new Name("meter"), attributes, children);

        public static element nav = (attributes, children)
            => element(new Name("nav"), attributes, children);

        public static element noembed = (attributes, children)
            => element(new Name("noembed"), attributes, children);

        public static element noframes = (attributes, children)
            => element(new Name("noframes"), attributes, children);

        public static element noscript = (attributes, children)
            => element(new Name("noscript"), attributes, children);

        public static element Object = (attributes, children)
            => element(new Name("object"), attributes, children);

        public static element ol = (attributes, children)
            => element(new Name("ol"), attributes, children);

        public static element optgroup = (attributes, children)
            => element(new Name("optgroup"), attributes, children);

        public static element option = (attributes, children)
            => element(new Name("option"), attributes, children);

        public static element output = (attributes, children)
            => element(new Name("output"), attributes, children);

        public static element p = (attributes, children)
            => element(new Name("p"), attributes, children);

        public static element param = (attributes, children)
            => element(new Name("param"), attributes, children);

        public static element picture = (attributes, children)
            => element(new Name("picture"), attributes, children);

        public static element pre = (attributes, children)
            => element(new Name("pre"), attributes, children);

        public static element progress = (attributes, children)
            => element(new Name("progress"), attributes, children);

        public static element q = (attributes, children)
            => element(new Name("q"), attributes, children);

        public static element rb = (attributes, children)
            => element(new Name("rb"), attributes, children);

        public static element rp = (attributes, children)
            => element(new Name("rp"), attributes, children);

        public static element rt = (attributes, children)
            => element(new Name("rt"), attributes, children);

        public static element rtc = (attributes, children)
            => element(new Name("rtc"), attributes, children);

        public static element ruby = (attributes, children)
            => element(new Name("ruby"), attributes, children);

        public static element s = (attributes, children)
            => element(new Name("s"), attributes, children);

        public static element samp = (attributes, children)
            => element(new Name("samp"), attributes, children);

        public static element script = (attributes, children)
            => element(new Name("script"), attributes, children);

        public static element section = (attributes, children)
            => element(new Name("section"), attributes, children);

        public static element select = (attributes, children)
            => element(new Name("select"), attributes, children);

        public static element shadow = (attributes, children)
            => element(new Name("shadow"), attributes, children);

        public static element slot = (attributes, children)
            => element(new Name("slot"), attributes, children);

        public static element small = (attributes, children)
            => element(new Name("small"), attributes, children);

        //public static node source = (attributes) 
        //    => Element(new Name("source"), attributes, Enumerable.Empty);

        public static element span = (attributes, children)
            => element(new Name("span"), attributes, children);

        public static element strike = (attributes, children)
            => element(new Name("strike"), attributes, children);

        public static element strong = (attributes, children)
            => element(new Name("strong"), attributes, children);

        public static element style = (attributes, children)
            => element(new Name("style"), attributes, children);

        public static element sub = (attributes, children)
            => element(new Name("sub"), attributes, children);

        public static element summary = (attributes, children)
            => element(new Name("summary"), attributes, children);

        public static element sup = (attributes, children)
            => element(new Name("sup"), attributes, children);

        public static element svg = (attributes, children)
            => element(new Name("svg"), attributes, children);

        public static element table = (attributes, children)
            => element(new Name("table"), attributes, children);

        public static element tbody = (attributes, children)
            => element(new Name("tbody"), attributes, children);

        public static element td = (attributes, children)
            => element(new Name("td"), attributes, children);

        public static element template = (attributes, children)
            => element(new Name("template"), attributes, children);

        public static element textarea = (attributes, children)
            => element(new Name("textarea"), attributes, children);

        public static element tfoot = (attributes, children)
            => element(new Name("tfoot"), attributes, children);

        public static element th = (attributes, children)
            => element(new Name("th"), attributes, children);

        public static element thead = (attributes, children)
            => element(new Name("thead"), attributes, children);

        public static element time = (attributes, children)
            => element(new Name("time"), attributes, children);

        public static element title = (attributes, children)
            => element(new Name("title"), attributes, children);

        public static element tr = (attributes, children)
            => element(new Name("tr"), attributes, children);

        public static element track = (attributes, children)
            => element(new Name("track"), attributes, children);

        public static element tt = (attributes, children)
            => element(new Name("tt"), attributes, children);

        public static element u = (attributes, children)
            => element(new Name("u"), attributes, children);

        public static element ul = (attributes, children)
            => element(new Name("ul"), attributes, children);

        public static element var = (attributes, children)
            => element(new Name("var"), attributes, children);

        public static element video = (attributes, children)
            => element(new Name("video"), attributes, children);

        public static element wbr = (attributes, children)
            => element(new Name("wbr"), attributes, children);

    }
}