namespace Radix.Web.Css.Data.Units.Angle;

/// <summary>
/// Represents an angle in degrees. One full circle is 360deg. Examples: 0deg, 90deg, 14.23deg.
/// </summary>
[Literal]
public partial struct deg : Unit { }

/// <summary>
/// Represents an angle in gradians. One full circle is 400grad. Examples: 0grad, 100grad, 38.8grad.
/// </summary>
[Literal]
public partial struct grad : Unit { }

/// <summary>
/// Represents an angle in radians. One full circle is 2π radians which approximates to 6.2832rad. 1rad is 180/π degrees. Examples: 0rad, 1.0708rad, 6.2832rad.
/// </summary>
[Literal]
public partial struct rad : Unit { }

/// <summary>
/// Represents an angle in a number of turns. One full circle is 1turn. Examples: 0turn, 0.25turn, 1.2turn.
/// </summary>
[Literal]
public partial struct turn : Unit { }
