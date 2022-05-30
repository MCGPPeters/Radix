using Radix.Interaction.Data;
using Radix.Web.Html.Data.Names.Elements;
using Attribute = Radix.Interaction.Data.Attribute;
namespace Radix.Interaction.Web.Components.Nodes;


public static class Elements
{
    public static Element element(NodeId nodeId, string name, Attribute[] attributes, params Node[] children) =>
        new(nodeId, name, attributes, children);

    public static Element<a> a(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<a> a(NodeId nodeId, params Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<a> a(NodeId nodeId, Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<a> a(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);


    public static Element<abbr> abbr(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<abbr> abbr(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<abbr> abbr(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<abbr> abbr(NodeId nodeId, Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<address> address(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<address> address(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<address> address(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<address> address(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<area> area(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<area> area(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<area> area(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<area> area(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<article> article(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<article> article(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<article> article(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<article> article(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<aside> aside(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<aside> aside(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<aside> aside(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<aside> aside(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<audio> audio(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<audio> audio(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<audio> audio(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<audio> audio(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<b> b(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<b> b(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<b> b(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<b> b(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<@base> @base(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<@base> @base(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<@base> @base(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<@base> @base(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<bdi> bdi(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<bdi> bdi(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<bdi> bdi(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<bdi> bdi(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<bdo> bdo(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<bdo> bdo(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<bdo> bdo(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<bdo> bdo(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<blockquote> blockquote(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<blockquote> blockquote(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<blockquote> blockquote(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<blockquote> blockquote(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<body> body(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<body> body(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<body> body(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<body> body(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<br> br(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<br> br(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<br> br(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<br> br(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<button> button(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<button> button(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<button> button(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<button> button(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<canvas> canvas(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<canvas> canvas(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<canvas> canvas(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<canvas> canvas(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<caption> caption(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<caption> caption(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<caption> caption(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<caption> caption(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);


    public static Element<code> code(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<code> code(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<code> code(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<code> code(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<col> col(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<col> col(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<col> col(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<col> col(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<colgroup> colgroup(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<colgroup> colgroup(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<colgroup> colgroup(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<colgroup> colgroup(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<data> data(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<data> data(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<data> data(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<data> data(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<datalist> datalist(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<datalist> datalist(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<datalist> datalist(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<datalist> datalist(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<dd> dd(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<dd> dd(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<dd> dd(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<del> del(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<del> del(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<del> del(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<del> del(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<details> details(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<details> details(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<details> details(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<details> details(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<dfn> dfn(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<dfn> dfn(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<dfn> dfn(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<dfn> dfn(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<dialog> dialog(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<dialog> dialog(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<dialog> dialog(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<dialog> dialog(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);
    public static Element<div> div(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<div> div(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<div> div(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<div> div(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<dl> dl(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<dl> dl(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<dl> dl(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<dl> dl(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<dt> dt(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<dt> dt(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<dt> dt(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<dt> dt(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<em> em(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<em> em(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<em> em(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<em> em(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<embed> embed(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<embed> embed(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<embed> embed(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<embed> embed(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<fieldset> fieldset(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<fieldset> fieldset(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<fieldset> fieldset(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<fieldset> fieldset(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<figcaption> figcaption(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<figcaption> figcaption(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<figcaption> figcaption(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<figcaption> figcaption(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<figure> figure(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<figure> figure(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<figure> figure(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<figure> figure(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<form> form(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<form> form(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<form> form(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<form> form(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<footer> footer(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<footer> footer(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<footer> footer(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<footer> footer(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
        new(nodeId, new[] { attribute }, children);

    public static Element<h1> h1(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h1> h1(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h1> h1(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h1> h1(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<h2> h2(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h2> h2(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h2> h2(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h2> h2(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<h3> h3(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h3> h3(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h3> h3(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h3> h3(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<h4> h4(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h4> h4(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h4> h4(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h4> h4(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<h5> h5(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h5> h5(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h5> h5(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h5> h5(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<h6> h6(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<h6> h6(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<h6> h6(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<h6> h6(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<head> head(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<head> head(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<head> head(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<head> head(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<header> header(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<header> header(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<header> header(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<header> header(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<hgroup> hgroup(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<hgroup> hgroup(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<hgroup> hgroup(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<hgroup> hgroup(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
       new(nodeId, new[] { attribute }, children);

    public static Element<hr> hr(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<hr> hr(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<hr> hr(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<hr> hr(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<html> html(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<html> html(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<html> html(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<html> html(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<i> i(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<i> i(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<i> i(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<i> i(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<iframe> iframe(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<iframe> iframe(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<iframe> iframe(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<iframe> iframe(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<img> img(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<img> img(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<img> img(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<img> img(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<input> input(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<input> input(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<input> input(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<input> input(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<ins> ins(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<ins> ins(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<ins> ins(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<ins> ins(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<kbd> kbd(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<kbd> kbd(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<kbd> kbd(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<kbd> kbd(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<label> label(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<label> label(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<label> label(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<label> label(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<legend> legend(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<legend> legend(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<legend> legend(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<legend> legend(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<li> li(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<li> li(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<li> li(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<li> li(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<link> link(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<link> link(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<link> link(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<link> link(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<main> main(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<main> main(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<main> main(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<main> main(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<map> map(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<map> map(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<map> map(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<map> map(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<mark> mark(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<mark> mark(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<mark> mark(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<mark> mark(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<math> math(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<math> math(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<math> math(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<math> math(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<menu> menu(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<menu> menu(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<menu> menu(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<menu> menu(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<meta> meta(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<meta> meta(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<meta> meta(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<meta> meta(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<meter> meter(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<meter> meter(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<meter> meter(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<meter> meter(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<nav> nav(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<nav> nav(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<nav> nav(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<nav> nav(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<noscript> noscript(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<noscript> noscript(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<noscript> noscript(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<noscript> noscript(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<@object> @object(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<@object> @object(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<@object> @object(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<@object> @object(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<ol> ol(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<ol> ol(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<ol> ol(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<ol> ol(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<optgroup> optgroup(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<optgroup> optgroup(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<optgroup> optgroup(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<optgroup> optgroup(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<option> option(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<option> option(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<option> option(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<option> option(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<output> output(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<output> output(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<output> output(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<output> output(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<p> p(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<p> p(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<p> p(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<p> p(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<param> param(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<param> param(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<param> param(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<param> param(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<picture> picture(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<picture> picture(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<picture> picture(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<picture> picture(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<pre> pre(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<pre> pre(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<pre> pre(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<pre> pre(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<progress> progress(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<progress> progress(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<progress> progress(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<progress> progress(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<q> q(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<q> q(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<q> q(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<q> q(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<rp> rp(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<rp> rp(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<rp> rp(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<rp> rp(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<rt> rt(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<rt> rt(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<rt> rt(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<rt> rt(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<ruby> ruby(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<ruby> ruby(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<ruby> ruby(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<ruby> ruby(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);
    public static Element<s> s(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<s> s(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<s> s(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<s> s(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<samp> samp(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<samp> samp(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<samp> samp(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<samp> samp(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<script> script(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<script> script(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<script> script(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<script> script(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<section> section(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<section> section(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<section> section(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<section> section(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<select> select(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<select> select(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<select> select(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<select> select(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<small> small(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<small> small(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<small> small(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<small> small(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<source> source(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<source> source(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<source> source(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<source> source(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<span> span(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<span> span(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<span> span(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<span> span(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<strong> strong(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<strong> strong(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<strong> strong(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<strong> strong(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<style> style(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<style> style(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<style> style(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<style> style(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<sub> sub(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<sub> sub(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<sub> sub(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<sub> sub(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<summary> summary(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<summary> summary(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<summary> summary(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<summary> summary(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<sup> sup(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<sup> sup(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<sup> sup(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<sup> sup(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<svg> svg(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<svg> svg(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<svg> svg(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<svg> svg(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<table> table(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<table> table(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<table> table(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<table> table(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<tbody> tbody(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<tbody> tbody(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<tbody> tbody(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<tbody> tbody(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<td> td(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<td> td(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<td> td(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<td> td(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<template> template(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<template> template(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<template> template(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<template> template(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<textarea> textarea(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<textarea> textarea(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<textarea> textarea(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<textarea> textarea(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<tfoot> tfoot(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<tfoot> tfoot(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<tfoot> tfoot(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<tfoot> tfoot(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<th> th(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<th> th(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<th> th(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<th> th(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<thead> thead(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<thead> thead(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<thead> thead(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<thead> thead(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<time> time(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<time> time(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<time> time(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<time> time(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<title> title(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<title> title(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<title> title(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<title> title(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<tr> tr(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<tr> tr(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<tr> tr(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<tr> tr(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);


    public static Element<track> track(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<track> track(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<track> track(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<track> track(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<u> u(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<u> u(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<u> u(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<u> u(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<ul> ul(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<ul> ul(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<ul> ul(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<ul> ul(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<@var> @var(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<@var> @var(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);
    public static Element<@var> @var(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);
    public static Element<@var> @var(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<video> video(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<video> video(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<video> video(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<video> video(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

    public static Element<wbr> wbr(NodeId nodeId, Data.Attribute[] attributes, params Node[] children) =>
        new(nodeId, attributes, children);

    public static Element<wbr> wbr(NodeId nodeId, params Node[] children) =>
        new(nodeId, children);

    public static Element<wbr> wbr(NodeId nodeId, params Data.Attribute[] attributes) =>
        new(nodeId, attributes);

    public static Element<wbr> wbr(NodeId nodeId, Data.Attribute attribute, params Node[] children) =>
    new(nodeId, new[] { attribute }, children);

}
