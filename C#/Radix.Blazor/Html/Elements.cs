using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public delegate Node element(IEnumerable<IAttribute> attributes, params Node[] children);

    public delegate Node element<in T>(params T[] attributes) where T : IAttribute;

    public static class Elements
    {

        public static element a = (attributes, children)
            => element(nameof(a), attributes, children);

        public static element abbr = (attributes, children)
            => element(nameof(abbr), attributes, children);

        public static element acronym = (attributes, children)
            => element(nameof(acronym), attributes, children);

        public static element address = (attributes, children)
            => element(nameof(address), attributes, children);

        public static element applet = (attributes, children)
            => element(nameof(applet), attributes, children);

        public static element area = (attributes, children)
            => element(nameof(area), attributes, children);

        public static element article = (attributes, children)
            => element(nameof(article), attributes, children);

        public static element aside = (attributes, children)
            => element(nameof(aside), attributes, children);

        public static element audio = (attributes, children)
            => element(nameof(audio), attributes, children);

        public static element b = (attributes, children)
            => element(nameof(b), attributes, children);

        public static element @base = (attributes, children)
            => element(nameof(@base), attributes, children);

        public static element basefont = (attributes, children)
            => element(nameof(basefont), attributes, children);

        public static element bdi = (attributes, children)
            => element(nameof(bdi), attributes, children);

        public static element bdo = (attributes, children)
            => element(nameof(bdo), attributes, children);

        public static element big = (attributes, children)
            => element(nameof(big), attributes, children);

        public static element<IAttribute> br = attributes
            => element(nameof(br), attributes);

        public static element button = (attributes, children)
            => element(nameof(button), attributes, children);

        public static element canvas = (attributes, children)
            => element(nameof(canvas), attributes, children);

        public static element caption = (attributes, children)
            => element(nameof(caption), attributes, children);

        public static element center = (attributes, children)
            => element(nameof(center), attributes, children);

        public static element cite = (attributes, children)
            => element(nameof(cite), attributes, children);

        public static element code = (attributes, children)
            => element(nameof(code), attributes, children);

        public static element col = (attributes, children)
            => element(nameof(col), attributes, children);

        public static element colgroup = (attributes, children)
            => element(nameof(colgroup), attributes, children);

        public static element content = (attributes, children)
            => element(nameof(content), attributes, children);

        public static element data = (attributes, children)
            => element(nameof(data), attributes, children);

        public static element datalist = (attributes, children)
            => element(nameof(datalist), attributes, children);

        public static element dd = (attributes, children)
            => element(nameof(dd), attributes, children);

        public static element del = (attributes, children)
            => element(nameof(del), attributes, children);

        public static element details = (attributes, children)
            => element(nameof(details), attributes, children);

        public static element dfn = (attributes, children)
            => element(nameof(dfn), attributes, children);

        public static element dialog = (attributes, children)
            => element(nameof(dialog), attributes, children);

        public static element dir = (attributes, children)
            => element(nameof(dir), attributes, children);

        public static element div = (attributes, children)
            => element(nameof(div), attributes, children);

        public static element dl = (attributes, children)
            => element(nameof(dl), attributes, children);

        public static element dt = (attributes, children)
            => element(nameof(dt), attributes, children);

        public static element em = (attributes, children)
            => element(nameof(em), attributes, children);

        public static element embed = (attributes, children)
            => element(nameof(embed), attributes, children);

        public static element fieldset = (attributes, children)
            => element(nameof(fieldset), attributes, children);

        public static element figcaption = (attributes, children)
            => element(nameof(figcaption), attributes, children);

        public static element figure = (attributes, children)
            => element(nameof(figure), attributes, children);

        public static element font = (attributes, children)
            => element(nameof(font), attributes, children);

        public static element footer = (attributes, children)
            => element(nameof(footer), attributes, children);

        public static element form = (attributes, children)
            => element(nameof(form), attributes, children);

        public static element frame = (attributes, children)
            => element(nameof(frame), attributes, children);

        public static element frameset = (attributes, children)
            => element(nameof(frameset), attributes, children);

        public static element h1 = (attributes, children)
            => element(nameof(h1), attributes, children);

        public static element h2 = (attributes, children)
            => element(nameof(h2), attributes, children);

        public static element h3 = (attributes, children)
            => element(nameof(h3), attributes, children);

        public static element h4 = (attributes, children)
            => element(nameof(h4), attributes, children);

        public static element h5 = (attributes, children)
            => element(nameof(h5), attributes, children);

        public static element h6 = (attributes, children)
            => element(nameof(h6), attributes, children);

        public static element head = (attributes, children)
            => element(nameof(head), attributes, children);

        public static element header = (attributes, children)
            => element(nameof(header), attributes, children);

        public static element hr = (attributes, children)
            => element(nameof(hr), attributes, children);

        public static element html = (attributes, children)
            => element(nameof(html), attributes, children);

        public static element i = (attributes, children)
            => element(nameof(i), attributes, children);

        public static element iframe = (attributes, children)
            => element(nameof(iframe), attributes, children);

        public static element img = (attributes, children)
            => element(nameof(img), attributes, children);

        public static element<IAttribute> input = attributes
            => element(nameof(input), attributes);

        public static element ins = (attributes, children)
            => element(nameof(ins), attributes, children);

        public static element kbd = (attributes, children)
            => element(nameof(kbd), attributes, children);

        public static element label = (attributes, children)
            => element(nameof(label), attributes, children);

        public static element legend = (attributes, children)
            => element(nameof(legend), attributes, children);

        public static element li = (attributes, children)
            => element(nameof(li), attributes, children);

        public static element link = (attributes, children)
            => element(nameof(link), attributes, children);

        public static element main = (attributes, children)
            => element(nameof(main), attributes, children);

        public static element map = (attributes, children)
            => element(nameof(map), attributes, children);

        public static element mark = (attributes, children)
            => element(nameof(mark), attributes, children);

        public static element menu = (attributes, children)
            => element(nameof(menu), attributes, children);

        public static element menuitem = (attributes, children)
            => element(nameof(menuitem), attributes, children);

        public static element meta = (attributes, children)
            => element(nameof(meta), attributes, children);

        public static element meter = (attributes, children)
            => element(nameof(meter), attributes, children);

        public static element nav = (attributes, children)
            => element(nameof(nav), attributes, children);

        public static element noembed = (attributes, children)
            => element(nameof(noembed), attributes, children);

        public static element noframes = (attributes, children)
            => element(nameof(noframes), attributes, children);

        public static element noscript = (attributes, children)
            => element(nameof(noscript), attributes, children);

        public static element Object = (attributes, children)
            => element(nameof(Object), attributes, children);

        public static element ol = (attributes, children)
            => element(nameof(ol), attributes, children);

        public static element optgroup = (attributes, children)
            => element(nameof(optgroup), attributes, children);

        public static element option = (attributes, children)
            => element(nameof(option), attributes, children);

        public static element output = (attributes, children)
            => element(nameof(output), attributes, children);

        public static element p = (attributes, children)
            => element(nameof(p), attributes, children);

        public static element param = (attributes, children)
            => element(nameof(param), attributes, children);

        public static element picture = (attributes, children)
            => element(nameof(picture), attributes, children);

        public static element pre = (attributes, children)
            => element(nameof(pre), attributes, children);

        public static element progress = (attributes, children)
            => element(nameof(progress), attributes, children);

        public static element q = (attributes, children)
            => element(nameof(q), attributes, children);

        public static element rb = (attributes, children)
            => element(nameof(rb), attributes, children);

        public static element rp = (attributes, children)
            => element(nameof(rp), attributes, children);

        public static element rt = (attributes, children)
            => element(nameof(rt), attributes, children);

        public static element rtc = (attributes, children)
            => element(nameof(rtc), attributes, children);

        public static element ruby = (attributes, children)
            => element(nameof(ruby), attributes, children);

        public static element s = (attributes, children)
            => element(nameof(s), attributes, children);

        public static element samp = (attributes, children)
            => element(nameof(samp), attributes, children);

        public static element script = (attributes, children)
            => element(nameof(script), attributes, children);

        public static element section = (attributes, children)
            => element(nameof(section), attributes, children);

        public static element select = (attributes, children)
            => element(nameof(select), attributes, children);

        public static element shadow = (attributes, children)
            => element(nameof(shadow), attributes, children);

        public static element slot = (attributes, children)
            => element(nameof(slot), attributes, children);

        public static element small = (attributes, children)
            => element(nameof(small), attributes, children);

        //public static node source = (attributes) 
        //    => element(nameof(source), attributes, Enumerable.empty);

        public static element span = (attributes, children)
            => element(nameof(span), attributes, children);

        public static element strike = (attributes, children)
            => element(nameof(strike), attributes, children);

        public static element strong = (attributes, children)
            => element(nameof(strong), attributes, children);

        public static element style = (attributes, children)
            => element(nameof(style), attributes, children);

        public static element sub = (attributes, children)
            => element(nameof(sub), attributes, children);

        public static element summary = (attributes, children)
            => element(nameof(summary), attributes, children);

        public static element sup = (attributes, children)
            => element(nameof(sup), attributes, children);

        public static element svg = (attributes, children)
            => element(nameof(svg), attributes, children);

        public static element table = (attributes, children)
            => element(nameof(table), attributes, children);

        public static element tbody = (attributes, children)
            => element(nameof(tbody), attributes, children);

        public static element td = (attributes, children)
            => element(nameof(td), attributes, children);

        public static element template = (attributes, children)
            => element(nameof(template), attributes, children);

        public static element textarea = (attributes, children)
            => element(nameof(textarea), attributes, children);

        public static element tfoot = (attributes, children)
            => element(nameof(tfoot), attributes, children);

        public static element th = (attributes, children)
            => element(nameof(th), attributes, children);

        public static element thead = (attributes, children)
            => element(nameof(thead), attributes, children);

        public static element time = (attributes, children)
            => element(nameof(time), attributes, children);

        public static element title = (attributes, children)
            => element(nameof(title), attributes, children);

        public static element tr = (attributes, children)
            => element(nameof(tr), attributes, children);

        public static element track = (attributes, children)
            => element(nameof(track), attributes, children);

        public static element tt = (attributes, children)
            => element(nameof(tt), attributes, children);

        public static element u = (attributes, children)
            => element(nameof(u), attributes, children);

        public static element ul = (attributes, children)
            => element(nameof(ul), attributes, children);

        public static element var = (attributes, children)
            => element(nameof(var), attributes, children);

        public static element video = (attributes, children)
            => element(nameof(video), attributes, children);

        public static element wbr = (attributes, children)
            => element(nameof(wbr), attributes, children);

        public static Empty empty
            => new Empty();

        public static Text text(string text)
        {
            return new Text(text);
        }

        public static Concat concat(params Node[] nodes)
        {
            return new Concat(nodes);
        }

        public static Element element(Name name, IEnumerable<IAttribute> attributes, params Node[] children)
        {
            return new Element(name, attributes, children);
        }

        public static Element element(Name name, params IAttribute[] attributes)
        {
            return new Element(name, attributes);
        }
    }
}
