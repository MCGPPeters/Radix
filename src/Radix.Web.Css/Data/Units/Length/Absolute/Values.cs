using Radix.Generators.Attributes;

namespace Radix.Web.Css.Data.Units.Length.Absolute;

/// <summary>
/// Centimeters, 1cm = 37.8px = 25.2/64in
/// </summary>
[Literal]
public partial struct cm : Unit<cm> { }

/// <summary>
/// Millimeters, 1mm = 1/10th of 1cm
/// </summary>
[Literal]
public partial struct mm : Unit<mm> { }

/// <summary>
/// Quarter-millimeters: 1Q = 1/40th of 1cm
/// </summary>
[Literal]
public partial struct Q : Unit<Q> { }

/// <summary>
/// Inches, 1in = 2.54cm = 96px
/// </summary>
[Literal(StringRepresentation = "in")]
public partial struct inches : Unit<inches> { }

/// <summary>
/// Picas, 1pc = 1/6th of 1in
/// </summary>
[Literal]
public partial struct pc : Unit<pc> { }

/// <summary>
/// Points, 1pt = 1/72th of 1in
/// </summary>
[Literal]
public partial struct pt : Unit<pt> { }

/// <summary>
/// Pixels, 1px = 1/96th of 1in
/// </summary>
[Literal]
public partial struct px : Unit<px> { }
