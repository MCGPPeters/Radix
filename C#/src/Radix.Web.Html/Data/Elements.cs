using Radix.Web.Html.Data.Nodes;
using Radix.Components;
using Radix.Web.Html.Data.Names.Elements;

namespace Radix.Web.Html;


public static class Elements
{
    public static Element element(string name, IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(name, attributes, children);

    public static Element<a> a(params Node[] children) =>
        new(children);

    public static Element<a> a(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<a> a(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<a> a(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);


    public static Element<abbr> abbr(params Node[] children) =>
        new(children);

    public static Element<abbr> abbr(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<abbr> abbr(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<abbr> abbr(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<address> address(params Node[] children) =>
        new(children);

    public static Element<address> address(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<address> address(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<address> address(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<area> area(params Node[] children) =>
        new(children);

    public static Element<area> area(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<area> area(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<area> area(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<article> article(params Node[] children) =>
        new(children);

    public static Element<article> article(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<article> article(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<article> article(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<aside> aside(params Node[] children) =>
        new(children);

    public static Element<aside> aside(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<aside> aside(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<aside> aside(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<audio> audio(params Node[] children) =>
        new(children);

    public static Element<audio> audio(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<audio> audio(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<audio> audio(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<b> b(params Node[] children) =>
        new(children);

    public static Element<b> b(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<b> b(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<b> b(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<@base> @base(params Node[] children) =>
        new(children);

    public static Element<@base> @base(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<@base> @base(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<@base> @base(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<bdi> bdi(params Node[] children) =>
        new(children);

    public static Element<bdi> bdi(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<bdi> bdi(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<bdi> bdi(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<bdo> bdo(params Node[] children) =>
        new(children);

    public static Element<bdo> bdo(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<bdo> bdo(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<bdo> bdo(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<blockquote> blockquote(params Node[] children) =>
        new(children);

    public static Element<blockquote> blockquote(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<blockquote> blockquote(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<blockquote> blockquote(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<body> body(params Node[] children) =>
        new(children);

    public static Element<body> body(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<body> body(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<body> body(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<br> br(params Node[] children) =>
        new(children);

    public static Element<br> br(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<br> br(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<br> br(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<button> button(params Node[] children) =>
        new(children);

    public static Element<button> button(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<button> button(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<button> button(Data.Attribute attribute, params Node[] children) =>
        new(new[]{ attribute }, children);

    public static Element<canvas> canvas(params Node[] children) =>
        new(children);

    public static Element<canvas> canvas(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<canvas> canvas(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<canvas> canvas(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<caption> caption(params Node[] children) =>
        new(children);

    public static Element<caption> caption(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<caption> caption(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<caption> caption(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);


    public static Element<code> code(params Node[] children) =>
        new(children);

    public static Element<code> code(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<code> code(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<code> code(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<col> col(params Node[] children) =>
        new(children);

    public static Element<col> col(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<col> col(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<col> col(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<colgroup> colgroup(params Node[] children) =>
        new(children);

    public static Element<colgroup> colgroup(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<colgroup> colgroup(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<colgroup> colgroup(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<data> data(params Node[] children) =>
        new(children);

    public static Element<data> data(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<data> data(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<data> data(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<datalist> datalist(params Node[] children) =>
        new(children);

    public static Element<datalist> datalist(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<datalist> datalist(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<datalist> datalist(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<dd> dd(params Node[] children) =>
        new(children);

    public static Element<dd> dd(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<dd> dd(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<del> del(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<del> del(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<del> del(params Node[] children) =>
        new(children);

    public static Element<del> del(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<details> details(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<details> details(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<details> details(params Node[] children) =>
        new(children);

    public static Element<details> details(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<dfn> dfn(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<dfn> dfn(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<dfn> dfn(params Node[] children) =>
        new(children);

    public static Element<dfn> dfn(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<dialog> dialog(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<dialog> dialog(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<dialog> dialog(params Node[] children) =>
        new(children);

    public static Element<dialog> dialog(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);
    public static Element<div> div(params Node[] children) =>
        new(children);

    public static Element<div> div(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<div> div(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<div> div(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<dl> dl(params Node[] children) =>
        new(children);

    public static Element<dl> dl(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<dl> dl(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<dl> dl(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<dt> dt(params Node[] children) =>
        new(children);

    public static Element<dt> dt(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<dt> dt(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<dt> dt(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<em> em(params Node[] children) =>
        new(children);

    public static Element<em> em(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<em> em(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<em> em(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<embed> embed(params Node[] children) =>
        new(children);

    public static Element<embed> embed(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<embed> embed(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<embed> embed(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<fieldset> fieldset(params Node[] children) =>
        new(children);

    public static Element<fieldset> fieldset(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<fieldset> fieldset(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<fieldset> fieldset(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<figcaption> figcaption(params Node[] children) =>
        new(children);

    public static Element<figcaption> figcaption(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<figcaption> figcaption(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<figcaption> figcaption(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<figure> figure(params Node[] children) =>
        new(children);

    public static Element<figure> figure(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<figure> figure(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<figure> figure(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<form> form(params Node[] children) =>
        new(children);

    public static Element<form> form(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<form> form(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<form> form(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<footer> footer(params Node[] children) =>
        new(children);

    public static Element<footer> footer(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<footer> footer(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<footer> footer(Data.Attribute attribute, params Node[] children) =>
        new(new[] { attribute }, children);

    public static Element<h1> h1(params Node[] children) =>
        new(children);

    public static Element<h1> h1(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h1> h1(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h1> h1(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<h2> h2(params Node[] children) =>
        new(children);

    public static Element<h2> h2(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h2> h2(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h2> h2(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<h3> h3(params Node[] children) =>
        new(children);

    public static Element<h3> h3(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h3> h3(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h3> h3(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<h4> h4(params Node[] children) =>
        new(children);

    public static Element<h4> h4(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h4> h4(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h4> h4(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<h5> h5(params Node[] children) =>
        new(children);

    public static Element<h5> h5(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h5> h5(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h5> h5(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<h6> h6(params Node[] children) =>
        new(children);

    public static Element<h6> h6(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<h6> h6(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<h6> h6(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<head> head(params Node[] children) =>
        new(children);

    public static Element<head> head(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<head> head(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<head> head(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<header> header(params Node[] children) =>
        new(children);

    public static Element<header> header(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<header> header(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<header> header(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<hgroup> hgroup(params Node[] children) =>
        new(children);

    public static Element<hgroup> hgroup(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<hgroup> hgroup(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<hgroup> hgroup(Data.Attribute attribute, params Node[] children) =>
       new(new[] { attribute }, children);

    public static Element<hr> hr(params Node[] children) =>
        new(children);

    public static Element<hr> hr(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<hr> hr(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<hr> hr(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<html> html(params Node[] children) =>
        new(children);

    public static Element<html> html(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<html> html(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<html> html(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<i> i(params Node[] children) =>
        new(children);

    public static Element<i> i(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<i> i(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<i> i(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<iframe> iframe(params Node[] children) =>
        new(children);

    public static Element<iframe> iframe(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<iframe> iframe(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<iframe> iframe(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<img> img(params Node[] children) =>
        new(children);

    public static Element<img> img(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<img> img(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<img> img(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<input> input(params Node[] children) =>
        new(children);

    public static Element<input> input(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<input> input(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<input> input(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<ins> ins(params Node[] children) =>
        new(children);

    public static Element<ins> ins(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<ins> ins(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<ins> ins(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<kbd> kbd(params Node[] children) =>
        new(children);

    public static Element<kbd> kbd(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<kbd> kbd(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<kbd> kbd(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<label> label(params Node[] children) =>
        new(children);

    public static Element<label> label(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<label> label(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<label> label(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<legend> legend(params Node[] children) =>
        new(children);

    public static Element<legend> legend(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<legend> legend(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<legend> legend(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<li> li(params Node[] children) =>
        new(children);

    public static Element<li> li(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<li> li(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<li> li(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<link> link(params Node[] children) =>
        new(children);

    public static Element<link> link(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<link> link(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<link> link(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<main> main(params Node[] children) =>
        new(children);

    public static Element<main> main(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<main> main(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<main> main(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<map> map(params Node[] children) =>
        new(children);

    public static Element<map> map(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<map> map(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<map> map(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<mark> mark(params Node[] children) =>
        new(children);

    public static Element<mark> mark(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<mark> mark(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<mark> mark(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<math> math(params Node[] children) =>
        new(children);

    public static Element<math> math(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<math> math(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<math> math(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<menu> menu(params Node[] children) =>
        new(children);

    public static Element<menu> menu(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<menu> menu(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<menu> menu(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<meta> meta(params Node[] children) =>
        new(children);

    public static Element<meta> meta(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<meta> meta(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<meta> meta(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<meter> meter(params Node[] children) =>
        new(children);

    public static Element<meter> meter(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<meter> meter(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<meter> meter(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<nav> nav(params Node[] children) =>
        new(children);

    public static Element<nav> nav(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<nav> nav(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<nav> nav(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<noscript> noscript(params Node[] children) =>
        new(children);

    public static Element<noscript> noscript(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<noscript> noscript(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<noscript> noscript(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<@object> @object(params Node[] children) =>
        new(children);

    public static Element<@object> @object(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<@object> @object(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<@object> @object(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<ol> ol(params Node[] children) =>
        new(children);

    public static Element<ol> ol(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<ol> ol(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<ol> ol(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<optgroup> optgroup(params Node[] children) =>
        new(children);

    public static Element<optgroup> optgroup(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<optgroup> optgroup(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<optgroup> optgroup(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<option> option(params Node[] children) =>
        new(children);

    public static Element<option> option(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<option> option(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<option> option(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<output> output(params Node[] children) =>
        new(children);

    public static Element<output> output(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<output> output(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<output> output(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<p> p(params Node[] children) =>
        new(children);

    public static Element<p> p(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<p> p(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<p> p(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<param> param(params Node[] children) =>
        new(children);

    public static Element<param> param(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<param> param(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<param> param(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<picture> picture(params Node[] children) =>
        new(children);

    public static Element<picture> picture(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<picture> picture(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<picture> picture(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<pre> pre(params Node[] children) =>
        new(children);

    public static Element<pre> pre(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<pre> pre(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<pre> pre(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<progress> progress(params Node[] children) =>
        new(children);

    public static Element<progress> progress(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<progress> progress(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<progress> progress(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<q> q(params Node[] children) =>
        new(children);

    public static Element<q> q(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<q> q(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<q> q(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<rp> rp(params Node[] children) =>
        new(children);

    public static Element<rp> rp(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<rp> rp(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<rp> rp(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<rt> rt(params Node[] children) =>
        new(children);

    public static Element<rt> rt(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<rt> rt(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<rt> rt(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<ruby> ruby(params Node[] children) =>
        new(children);

    public static Element<ruby> ruby(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<ruby> ruby(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<ruby> ruby(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);
    public static Element<s> s(params Node[] children) =>
        new(children);

    public static Element<s> s(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<s> s(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<s> s(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<samp> samp(params Node[] children) =>
        new(children);

    public static Element<samp> samp(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<samp> samp(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<samp> samp(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<script> script(params Node[] children) =>
        new(children);

    public static Element<script> script(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<script> script(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<script> script(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<section> section(params Node[] children) =>
        new(children);

    public static Element<section> section(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<section> section(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<section> section(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<select> select(params Node[] children) =>
        new(children);

    public static Element<select> select(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<select> select(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<select> select(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<small> small(params Node[] children) =>
        new(children);

    public static Element<small> small(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<small> small(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<small> small(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<source> source(params Node[] children) =>
        new(children);

    public static Element<source> source(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<source> source(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<source> source(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<span> span(params Node[] children) =>
        new(children);

    public static Element<span> span(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<span> span(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<span> span(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<strong> strong(params Node[] children) =>
        new(children);

    public static Element<strong> strong(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<strong> strong(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<strong> strong(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<style> style(params Node[] children) =>
        new(children);

    public static Element<style> style(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<style> style(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<style> style(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<sub> sub(params Node[] children) =>
        new(children);

    public static Element<sub> sub(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<sub> sub(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<sub> sub(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<summary> summary(params Node[] children) =>
        new(children);

    public static Element<summary> summary(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<summary> summary(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<summary> summary(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<sup> sup(params Node[] children) =>
        new(children);

    public static Element<sup> sup(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<sup> sup(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<sup> sup(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<svg> svg(params Node[] children) =>
        new(children);

    public static Element<svg> svg(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<svg> svg(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<svg> svg(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<table> table(params Node[] children) =>
        new(children);

    public static Element<table> table(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<table> table(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<table> table(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<tbody> tbody(params Node[] children) =>
        new(children);

    public static Element<tbody> tbody(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<tbody> tbody(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<tbody> tbody(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<td> td(params Node[] children) =>
        new(children);

    public static Element<td> td(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<td> td(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<td> td(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<template> template(params Node[] children) =>
        new(children);

    public static Element<template> template(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<template> template(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<template> template(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<textarea> textarea(params Node[] children) =>
        new(children);

    public static Element<textarea> textarea(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<textarea> textarea(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<textarea> textarea(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<tfoot> tfoot(params Node[] children) =>
        new(children);

    public static Element<tfoot> tfoot(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<tfoot> tfoot(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<tfoot> tfoot(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<th> th(params Node[] children) =>
        new(children);

    public static Element<th> th(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<th> th(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<th> th(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<thead> thead(params Node[] children) =>
        new(children);

    public static Element<thead> thead(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<thead> thead(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<thead> thead(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<time> time(params Node[] children) =>
        new(children);

    public static Element<time> time(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<time> time(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<time> time(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<title> title(params Node[] children) =>
        new(children);

    public static Element<title> title(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<title> title(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<title> title(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<tr> tr(params Node[] children) =>
        new(children);

    public static Element<tr> tr(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<tr> tr(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<tr> tr(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);


    public static Element<track> track(params Node[] children) =>
        new(children);

    public static Element<track> track(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<track> track(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<track> track(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<u> u(params Node[] children) =>
        new(children);

    public static Element<u> u(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<u> u(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<u> u(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<ul> ul(params Node[] children) =>
        new(children);

    public static Element<ul> ul(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<ul> ul(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<ul> ul(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<@var> @var(params Node[] children) =>
        new(children);

    public static Element<@var> @var(params Data.Attribute[] attributes) =>
        new(attributes);
    public static Element<@var> @var(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);
    public static Element<@var> @var(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<video> video(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<video> video(params Node[] children) =>
        new(children);

    public static Element<video> video(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<video> video(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

    public static Element<wbr> wbr(IEnumerable<Data.Attribute> attributes, params Node[] children) =>
        new(attributes, children);

    public static Element<wbr> wbr(params Node[] children) =>
        new(children);

    public static Element<wbr> wbr(params Data.Attribute[] attributes) =>
        new(attributes);

    public static Element<wbr> wbrdd(Data.Attribute attribute, params Node[] children) =>
    new(new[] { attribute }, children);

}
