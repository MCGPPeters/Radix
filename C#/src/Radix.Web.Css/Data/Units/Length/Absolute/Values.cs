namespace Radix.Web.Css.Data.Units.Length.Absolute;

/// <summary>
/// Centimeters, 1cm = 37.8px = 25.2/64in
/// </summary>
[Literal]
public partial struct cm : Unit { }

/// <summary>
/// Millimeters, 1mm = 1/10th of 1cm
/// </summary>
[Literal]
public partial struct mm : Unit { }

/// <summary>
/// Quarter-millimeters: 1Q = 1/40th of 1cm
/// </summary>
[Literal]
public partial struct Q : Unit { }

/// <summary>
/// Inches, 1in = 2.54cm = 96px
/// </summary>
[Literal(StringRepresentation = "in")]
public partial struct inches : Unit { }

/// <summary>
/// Picas, 1pc = 1/6th of 1in
/// </summary>
[Literal]
public partial struct pc : Unit { }

/// <summary>
/// Points, 1pt = 1/72th of 1in
/// </summary>
[Literal]
public partial struct pt : Unit { }

/// <summary>
/// Pixels, 1px = 1/96th of 1in
/// </summary>
[Literal]
public partial struct px : Unit { }
