using System.Runtime.CompilerServices;
using Radix.Interaction.Data;
using Radix.Web.Html.Data.Names.Elements;
using Attribute = Radix.Interaction.Data.Attribute;
namespace Radix.Interaction.Web.Components.Nodes;


public static class Elements
{
    public static Element element(string name, Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, name, attributes, children);

    public static Element<a> a(Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<abbr> abbr(Data.Attribute[] attributes,  Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<address> address(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);        

    public static Element<area> area(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<article> article(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<aside> aside(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<audio> audio(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<b> b(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<@base> @base(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<bdi> bdi(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<bdo> bdo(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<blockquote> blockquote(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);       

    public static Element<body> body(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<br> br(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<button> button(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<canvas> canvas(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<caption> caption(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<code> code(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<col> col(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<colgroup> colgroup(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<data> data(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<datalist> datalist(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<dd> dd(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<del> del(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<details> details(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<dfn> dfn(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<dialog> dialog(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<div> div(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<dl> dl(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<dt> dt(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<em> em(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<embed> embed(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<fieldset> fieldset(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<figcaption> figcaption(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<figure> figure(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<form> form(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<footer> footer(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h1> h1(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h2> h2(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h3> h3(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h4> h4(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h5> h5(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<h6> h6(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<head> head(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<header> header(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<hgroup> hgroup(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<hr> hr(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<html> html(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<i> i(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<iframe> iframe(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<img> img(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<input> input(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<ins> ins(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<kbd> kbd(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<label> label(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<legend> legend(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<li> li(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<link> link(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<main> main(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<map> map(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<mark> mark(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<math> math(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<menu> menu(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<meta> meta(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<meter> meter(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<nav> nav(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<noscript> noscript(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<@object> @object(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<ol> ol(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<optgroup> optgroup(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<option> option(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<output> output(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<p> p(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<param> param(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<picture> picture(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<pre> pre(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<progress> progress(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<q> q(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<rp> rp(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<rt> rt(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<ruby> ruby(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<s> s(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<samp> samp(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    

    public static Element<script> script(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<section> section(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<select> select(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<small> small(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<source> source(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<span> span(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
   


    public static Element<strong> strong(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
 
    public static Element<style> style(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<sub> sub(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<summary> summary(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<sup> sup(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<svg> svg(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    
    public static Element<table> table(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<tbody> tbody(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<td> td(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<template> template(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<textarea> textarea(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<tfoot> tfoot(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<th> th(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<thead> thead(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<time> time(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<title> title(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<tr> tr(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    

    public static Element<track> track(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);
    public static Element<u> u(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<ul> ul(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<@var> @var(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);

    public static Element<video> video(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


    public static Element<wbr> wbr(Data.Attribute[] attributes, Node[] children,  [CallerLineNumber] int nodeId = 0) =>
        new((NodeId)nodeId, attributes, children);


}
