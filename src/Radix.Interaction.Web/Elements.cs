using Radix.Web.Html.Data.Names.Elements;

using Element = Radix.Interaction.Element;

using static Radix.Interaction.Element;

using Radix.Interaction.Data;

using System.Runtime.CompilerServices;
// ReSharper disable InconsistentNaming


namespace Radix.Interaction.Web;


public static class Elements
{

    public static Node a(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) =>
        Create<a>(attributes, children, key, nodeId);
    public static Node a(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) =>
        Create<a>(children, key, nodeId);

    public static Node abbr(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<abbr>(attributes, children, key, nodeId);
   //public static Node abbr(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<abbr>(children, key, nodeId);
    public static Node abbr(Data.Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<abbr>(children, key, nodeId); 

    public static Node address(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<address>(attributes, children, key, nodeId);
   //public static Node address(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<address>(children, key, nodeId);
    public static Node address(Data.Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<address>(children, key, nodeId); 

    public static Node area(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<area>(attributes, children, key, nodeId);
   //public static Node area(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<area>(children, key, nodeId);
    public static Node area(Data.Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<area>(children, key, nodeId); 

    public static Node article(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<article>(attributes, children, key, nodeId);
    public static Node article(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<article>(children, key, nodeId);
    //public static Node article(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<article>(attributes, key, nodeId); 

    public static Node audio(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<audio>(attributes, children, key, nodeId);
    public static Node audio(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<audio>(children, key, nodeId);
    //public static Node audio(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<audio>(attributes, key, nodeId); 

    public static Node aside(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<aside>(attributes, children, key, nodeId);
    public static Node aside(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<aside>(children, key, nodeId);
    //public static Node aside(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<aside>(attributes, key, nodeId); 

    public static Node b(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<b>(attributes, children, key, nodeId);
    public static Node b(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<b>(children, key, nodeId);
    //public static Node b(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<b>(attributes, key, nodeId); 

    public static Node @base(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@base>(attributes, children, key, nodeId);
    public static Node @base(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@base>(children, key, nodeId);
    //public static Node @base(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@base>(attributes, key, nodeId); 

    public static Node bdi(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdi>(attributes, children, key, nodeId);
    public static Node bdi(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdi>(children, key, nodeId);
    //public static Node bdi(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdi>(attributes, key, nodeId); 

    public static Node bdo(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdo>(attributes, children, key, nodeId);
    public static Node bdo(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdo>(children, key, nodeId);
    //public static Node bdo(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<bdo>(attributes, key, nodeId); 

    public static Node blockquote(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<blockquote>(attributes, children, key, nodeId);
    public static Node blockquote(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<blockquote>(children, key, nodeId);
    //public static Node blockquote(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<blockquote>(attributes, key, nodeId); 

    public static Node body(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<body>(attributes, children, key, nodeId);
    public static Node body(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<body>(children, key, nodeId);
    //public static Node body(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<body>(attributes, key, nodeId); 

    public static Node br(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<br>(attributes, children, key, nodeId);
    public static Node br(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<br>(children, key, nodeId);
    //public static Node br(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<br>(attributes, key, nodeId); 

    public static Node button(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<button>(attributes, children, key, nodeId);
    public static Node button(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<button>(children, key, nodeId);
    //public static Node button(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<button>(attributes, key, nodeId); 


    public static Node canvas(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<canvas>(attributes, children, key, nodeId);
    public static Node canvas(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<canvas>(children, key, nodeId);
    //public static Node canvas(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<canvas>(attributes, key, nodeId); 

    public static Node caption(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<caption>(attributes, children, key, nodeId);
    public static Node caption(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<caption>(children, key, nodeId);
    //public static Node caption(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<caption>(attributes, key, nodeId); 

    public static Node code(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<code>(attributes, children, key, nodeId);
    public static Node code(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<code>(children, key, nodeId);
    //public static Node code(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<code>(attributes, key, nodeId); 

    public static Node col(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<col>(attributes, children, key, nodeId);
    public static Node col(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<col>(children, key, nodeId);
    //public static Node col(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<col>(attributes, key, nodeId); 

    public static Node colgroup(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<colgroup>(attributes, children, key, nodeId);
    public static Node colgroup(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<colgroup>(children, key, nodeId);
    //public static Node colgroup(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<colgroup>(attributes, key, nodeId); 

    public static Node data(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<data>(attributes, children, key, nodeId);
    public static Node data(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<data>(children, key, nodeId);
    //public static Node data(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<data>(attributes, key, nodeId); 

    public static Node datalist(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<datalist>(attributes, children, key, nodeId);
    public static Node datalist(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<datalist>(children, key, nodeId);
    //public static Node datalist(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<datalist>(attributes, key, nodeId); 

    public static Node dd(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dd>(attributes, children, key, nodeId);
    public static Node dd(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dd>(children, key, nodeId);
    //public static Node dd(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dd>(attributes, key, nodeId); 

    public static Node del(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<del>(attributes, children, key, nodeId);
    public static Node del(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<del>(children, key, nodeId);
    //public static Node del(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<del>(attributes, key, nodeId); 

    public static Node details(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<details>(attributes, children, key, nodeId);
    public static Node details(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<details>(children, key, nodeId);
    //public static Node details(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<details>(attributes, key, nodeId); 

    public static Node dfn(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dfn>(attributes, children, key, nodeId);
    public static Node dfn(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dfn>(children, key, nodeId);
    //public static Node dfn(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dfn>(attributes, key, nodeId); 

    public static Node dialog(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dialog>(attributes, children, key, nodeId);
    public static Node dialog(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dialog>(children, key, nodeId);
    //public static Node dialog(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dialog>(attributes, key, nodeId); 

    public static Node div(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<div>(attributes, children, key, nodeId);
    public static Node div(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<div>(children, key, nodeId);
    //public static Node div(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<div>(attributes, key, nodeId); 

    public static Node dl(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dl>(attributes, children, key, nodeId);
    public static Node dl(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dl>(children, key, nodeId);
    //public static Node dl(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dl>(attributes, key, nodeId); 

    public static Node dt(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dt>(attributes, children, key, nodeId);
    public static Node dt(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dt>(children, key, nodeId);
    //public static Node dt(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<dt>(attributes, key, nodeId); 

    public static Node em(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<em>(attributes, children, key, nodeId);
    public static Node em(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<em>(children, key, nodeId);
    //public static Node em(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<em>(attributes, key, nodeId); 

    public static Node embed(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<embed>(attributes, children, key, nodeId);
    public static Node embed(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<embed>(children, key, nodeId);
    //public static Node embed(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<embed>(attributes, key, nodeId); 


    public static Node fieldset(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<fieldset>(attributes, children, key, nodeId);
    public static Node fieldset(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<fieldset>(children, key, nodeId);
    //public static Node fieldset(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<fieldset>(attributes, key, nodeId); 


    public static Node figcaption(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figcaption>(attributes, children, key, nodeId);
    public static Node figcaption(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figcaption>(children, key, nodeId);
    //public static Node figcaption(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figcaption>(attributes, key, nodeId); 


    public static Node figure(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figure>(attributes, children, key, nodeId);
    public static Node figure(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figure>(children, key, nodeId);
    //public static Node figure(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<figure>(attributes, key, nodeId); 


    public static Node form(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<form>(attributes, children, key, nodeId);
    public static Node form(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<form>(children, key, nodeId);
    //public static Node form(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<form>(attributes, key, nodeId); 


    public static Node footer(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<footer>(attributes, children, key, nodeId);
    public static Node footer(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<footer>(children, key, nodeId);
    //public static Node footer(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<footer>(attributes, key, nodeId); 


    public static Node h1(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h1>(attributes, children, key, nodeId);
    public static Node h1(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h1>(children, key, nodeId);
    //public static Node h1(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h1>(attributes, key, nodeId); 


    public static Node h2(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h2>(attributes, children, key, nodeId);
    public static Node h2(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h2>( children, key, nodeId);


    public static Node h3(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h3>(attributes, children, key, nodeId);
    public static Node h3( Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h3>(children, key, nodeId);


    public static Node h4(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h4>(attributes, children, key, nodeId);
    public static Node h4(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h4>( children, key, nodeId);


    public static Node h5(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h5>(attributes, children, key, nodeId);
    public static Node h5(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h5>(children, key, nodeId);
    //public static Node h5(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h5>(attributes, key, nodeId); 


    public static Node h6(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h6>(attributes, children, key, nodeId);
    public static Node h6(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h6>(children, key, nodeId);
    //public static Node h6(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<h6>(attributes, key, nodeId); 


    public static Node head(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<head>(attributes, children, key, nodeId);
    public static Node head(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<head>(children, key, nodeId);
    //public static Node head(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<head>(attributes, key, nodeId); 

    public static Node header(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<header>(attributes, children, key, nodeId);
    public static Node header(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<header>(children, key, nodeId);
    //public static Node header(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<header>(attributes, key, nodeId); 


    public static Node hgroup(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hgroup>(attributes, children, key, nodeId);
    public static Node hgroup(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hgroup>(children, key, nodeId);
    //public static Node hgroup(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hgroup>(attributes, key, nodeId); 


    public static Node hr(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hr>(attributes, children, key, nodeId);
    public static Node hr(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hr>(children, key, nodeId);
    //public static Node hr(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<hr>(attributes, key, nodeId); 


    public static Node html(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<html>(attributes, children, key, nodeId);
    public static Node html(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<html>(children, key, nodeId);
    //public static Node html(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<html>(attributes, key, nodeId); 


    public static Node i(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<i>(attributes, children, key, nodeId);
    public static Node i(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<i>(children, key, nodeId);
    //public static Node i(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<i>(attributes, key, nodeId); 


    public static Node iframe(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<iframe>(attributes, children, key, nodeId);
    public static Node iframe(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<iframe>(children, key, nodeId);
    //public static Node iframe(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<iframe>(attributes, key, nodeId); 


    public static Node img(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<img>(attributes, children, key, nodeId);
    public static Node img(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<img>(children, key, nodeId);
    //public static Node img(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<img>(attributes, key, nodeId); 


    public static Node input(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<input>(attributes, children, key, nodeId);
    public static Node input(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<input>(children, key, nodeId);
    //public static Node input(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<input>(attributes, key, nodeId); 


    public static Node ins(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ins>(attributes, children, key, nodeId);
    public static Node ins(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ins>(children, key, nodeId);
    //public static Node ins(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ins>(attributes, key, nodeId); 


    public static Node kbd(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<kbd>(attributes, children, key, nodeId);
    public static Node kbd(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<kbd>(children, key, nodeId);
    //public static Node kbd(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<kbd>(attributes, key, nodeId); 


    public static Node label(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<label>(attributes, children, key, nodeId);
    public static Node label(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<label>(children, key, nodeId);
    //public static Node label(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<label>(attributes, key, nodeId); 



    public static Node legend(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<legend>(attributes, children, key, nodeId);
    public static Node legend(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<legend>(children, key, nodeId);
    //public static Node legend(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<legend>(attributes, key, nodeId); 


    public static Node li(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<li>(attributes, children, key, nodeId);
    public static Node li(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<li>(children, key, nodeId);
    //public static Node li(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<li>(attributes, key, nodeId); 

    public static Node link(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<link>(attributes, children, key, nodeId);
    public static Node link(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<link>(children, key, nodeId);
    //public static Node link(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<link>(attributes, key, nodeId); 


    public static Node main(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<main>(attributes, children, key, nodeId);
    public static Node main(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<main>(children, key, nodeId);
    //public static Node main(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<main>(attributes, key, nodeId); 


    public static Node map(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<map>(attributes, children, key, nodeId);
    public static Node map(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<map>(children, key, nodeId);
    //public static Node map(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<map>(attributes, key, nodeId); 


    public static Node mark(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<mark>(attributes, children, key, nodeId);
    public static Node mark(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<mark>(children, key, nodeId);
    //public static Node mark(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<mark>(attributes, key, nodeId); 



    public static Node math(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<math>(attributes, children, key, nodeId);
    public static Node math(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<math>(children, key, nodeId);
    //public static Node math(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<math>(attributes, key, nodeId); 


    public static Node menu(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<menu>(attributes, children, key, nodeId);
    public static Node menu(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<menu>(children, key, nodeId);
    //public static Node menu(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<menu>(attributes, key, nodeId); 


    public static Node meta(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meta>(attributes, children, key, nodeId);
    public static Node meta(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meta>(children, key, nodeId);
    //public static Node meta(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meta>(attributes, key, nodeId); 



    public static Node meter(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meter>(attributes, children, key, nodeId);
    public static Node meter(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meter>(children, key, nodeId);
    //public static Node meter(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<meter>(attributes, key, nodeId); 



    public static Node nav(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<nav>(attributes, children, key, nodeId);
    public static Node nav(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<nav>(children, key, nodeId);
    //public static Node nav(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<nav>(attributes, key, nodeId); 


    public static Node noscript(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<noscript>(attributes, children, key, nodeId);
    public static Node noscript(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<noscript>(children, key, nodeId);
    //public static Node noscript(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<noscript>(attributes, key, nodeId); 


    public static Node @object(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@object>(attributes, children, key, nodeId);
    public static Node @object(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@object>(children, key, nodeId);
    //public static Node @object(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@object>(attributes, key, nodeId); 

    public static Node ol(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ol>(attributes, children, key, nodeId);
    public static Node ol(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ol>(children, key, nodeId);
    //public static Node ol(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ol>(attributes, key, nodeId); 


    public static Node optgroup(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<optgroup>(attributes, children, key, nodeId);
    public static Node optgroup(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<optgroup>(children, key, nodeId);
    //public static Node optgroup(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<optgroup>(attributes, key, nodeId); 


    public static Node option(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<option>(attributes, children, key, nodeId);
    public static Node option(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<option>(children, key, nodeId);
    //public static Node option(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<option>(attributes, key, nodeId); 


    public static Node output(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<output>(attributes, children, key, nodeId);
    public static Node output(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<output>(children, key, nodeId);
    //public static Node output(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<output>(attributes, key, nodeId); 


    public static Node p(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<p>(attributes, children, key, nodeId);
    public static Node p(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<p>(children, key, nodeId);
    //public static Node p(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<p>(attributes, key, nodeId); 


    public static Node param(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<param>(attributes, children, key, nodeId);
    public static Node param(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<param>(children, key, nodeId);
    //public static Node param(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<param>(attributes, key, nodeId); 


    public static Node picture(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<picture>(attributes, children, key, nodeId);
    public static Node picture(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<picture>(children, key, nodeId);
    //public static Node picture(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<picture>(attributes, key, nodeId); 


    public static Node pre(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<pre>(attributes, children, key, nodeId);
    public static Node pre(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<pre>(children, key, nodeId);
    //public static Node pre(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<pre>(attributes, key, nodeId); 


    public static Node progress(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<progress>(attributes, children, key, nodeId);
    public static Node progress(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<progress>(children, key, nodeId);
    //public static Node progress(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<progress>(attributes, key, nodeId); 


    public static Node q(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<q>(attributes, children, key, nodeId);
    public static Node q(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<q>(children, key, nodeId);
    //public static Node q(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<q>(attributes, key, nodeId); 


    public static Node rp(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rp>(attributes, children, key, nodeId);
    public static Node rp(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rp>(children, key, nodeId);
    //public static Node rp(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rp>(attributes, key, nodeId); 


    public static Node rt(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rt>(attributes, children, key, nodeId);
    public static Node rt(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rt>(children, key, nodeId);
    //public static Node rt(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<rt>(attributes, key, nodeId); 


    public static Node ruby(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ruby>(attributes, children, key, nodeId);
    public static Node ruby(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ruby>(children, key, nodeId);
    //public static Node ruby(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ruby>(attributes, key, nodeId); 


    public static Node s(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<s>(attributes, children, key, nodeId);
    public static Node s(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<s>(children, key, nodeId);
    //public static Node s(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<s>(attributes, key, nodeId); 

    public static Node samp(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<samp>(attributes, children, key, nodeId);
    public static Node samp(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<samp>(children, key, nodeId);
    //public static Node samp(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<samp>(attributes, key, nodeId); 



    public static Node script(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<script>(attributes, children, key, nodeId);
    public static Node script(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<script>(children, key, nodeId);
    //public static Node script(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<script>(attributes, key, nodeId); 


    public static Node section(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<section>(attributes, children, key, nodeId);
    public static Node section(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<section>(children, key, nodeId);
    //public static Node section(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<section>(attributes, key, nodeId); 



    public static Node select(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<select>(attributes, children, key, nodeId);
    public static Node select(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<select>(children, key, nodeId);
    //public static Node select(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<select>(attributes, key, nodeId); 



    public static Node small(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<small>(attributes, children, key, nodeId);
    public static Node small(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<small>(children, key, nodeId);
    //public static Node small(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<small>(attributes, key, nodeId); 


    public static Node source(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<source>(attributes, children, key, nodeId);
    public static Node source(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<source>(children, key, nodeId);
    //public static Node source(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<source>(attributes, key, nodeId); 


    public static Node span(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<span>(attributes, children, key, nodeId);
    public static Node span(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<span>(children, key, nodeId);
    //public static Node span(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<span>(attributes, key, nodeId); 




    public static Node strong(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<strong>(attributes, children, key, nodeId);
    public static Node strong(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<strong>(children, key, nodeId);
    //public static Node strong(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<strong>(attributes, key, nodeId); 


    public static Node style(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<style>(attributes, children, key, nodeId);
    public static Node style(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<style>(children, key, nodeId);
    //public static Node style(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<style>(attributes, key, nodeId); 


    public static Node sub(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sub>(attributes, children, key, nodeId);
    public static Node sub(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sub>(children, key, nodeId);
    //public static Node sub(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sub>(attributes, key, nodeId); 


    public static Node summary(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<summary>(attributes, children, key, nodeId);
    public static Node summary(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<summary>(children, key, nodeId);
    //public static Node summary(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<summary>(attributes, key, nodeId); 


    public static Node sup(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sup>(attributes, children, key, nodeId);
    public static Node sup(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sup>(children, key, nodeId);
    //public static Node sup(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<sup>(attributes, key, nodeId); 


    public static Node svg(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<svg>(attributes, children, key, nodeId);
    public static Node svg(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<svg>(children, key, nodeId);
    //public static Node svg(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<svg>(attributes, key, nodeId); 


    public static Node table(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<table>(attributes, children, key, nodeId);
    public static Node table(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<table>(children, key, nodeId);
    //public static Node table(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<table>(attributes, key, nodeId); 



    public static Node tbody(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tbody>(attributes, children, key, nodeId);
    public static Node tbody(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tbody>(children, key, nodeId);
    //public static Node tbody(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tbody>(attributes, key, nodeId); 



    public static Node td(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<td>(attributes, children, key, nodeId);
    public static Node td(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<td>(children, key, nodeId);
    //public static Node td(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<td>(attributes, key, nodeId); 



    public static Node template(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<template>(attributes, children, key, nodeId);
    public static Node template(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<template>(children, key, nodeId);
    //public static Node template(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<template>(attributes, key, nodeId); 



    public static Node textarea(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<textarea>(attributes, children, key, nodeId);
    public static Node textarea(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<textarea>(children, key, nodeId);
    //public static Node textarea(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<textarea>(attributes, key, nodeId); 



    public static Node tfoot(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tfoot>(attributes, children, key, nodeId);
    public static Node tfoot(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tfoot>(children, key, nodeId);
    //public static Node tfoot(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tfoot>(attributes, key, nodeId); 


    public static Node th(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<th>(attributes, children, key, nodeId);
    public static Node th(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<th>(children, key, nodeId);
    //public static Node th(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<th>(attributes, key, nodeId); 


    public static Node thead(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<thead>(attributes, children, key, nodeId);
    public static Node thead(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<thead>(children, key, nodeId);
    //public static Node thead(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<thead>(attributes, key, nodeId); 

    public static Node time(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<time>(attributes, children, key, nodeId);
    public static Node time(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<time>(children, key, nodeId);
    //public static Node time(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<time>(attributes, key, nodeId); 


    public static Node title(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<title>(attributes, children, key, nodeId);
    public static Node title(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<title>(children, key, nodeId);
    //public static Node title(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<title>(attributes, key, nodeId); 


    public static Node tr(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tr>(attributes, children, key, nodeId);
    public static Node tr(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tr>(children, key, nodeId);
    //public static Node tr(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<tr>(attributes, key, nodeId); 



    public static Node track(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<track>(attributes, children, key, nodeId);
    public static Node track(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<track>(children, key, nodeId);
    //public static Node track(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<track>(attributes, key, nodeId); 

    public static Node u(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<u>(attributes, children, key, nodeId);
    public static Node u(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<u>(children, key, nodeId);
    //public static Node u(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<u>(attributes, key, nodeId); 



    public static Node ul(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ul>(attributes, children, key, nodeId);
    public static Node ul(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ul>(children, key, nodeId);
    //public static Node ul(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<ul>(attributes, key, nodeId); 


    public static Node @var(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@var>(attributes, children, key, nodeId);
    public static Node @var(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@var>(children, key, nodeId);
    //public static Node @var(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<@var>(attributes, key, nodeId); 

    public static Node video(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<video>(attributes, children, key, nodeId);
    public static Node video(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<video>(children, key, nodeId);
    //public static Node video(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<video>(attributes, key, nodeId); 



    public static Node wbr(Data.Attribute[] attributes, Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<wbr>(attributes, children, key, nodeId);
    public static Node wbr(Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<wbr>(children, key, nodeId);
    //public static Node wbr(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0) => Create<wbr>(attributes, key, nodeId); 

}
