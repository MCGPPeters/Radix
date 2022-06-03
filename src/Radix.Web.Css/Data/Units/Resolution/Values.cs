namespace Radix.Web.Css.Data.Units.Time;

/// <summary>
/// Represents the number of dots per inch. Screens typically contains 72 or 96 dots per inch, but the dpi for printed documents is usually much greater. As 1 inch is 2.54 cm, 1dpi ≈ 0.39dpcm.
/// </summary>
[Literal]
public partial struct dpi : Unit<dpi> { }

/// <summary>
/// Represents the number of dots per centimeter. As 1 inch is 2.54 cm, 1dpcm ≈ 2.54dpi.
/// </summary>
[Literal]
public partial struct dpcm : Unit<dpcm> { }

/// <summary>
/// Represents the number of dots per px unit. Due to the 1:96 fixed ratio of CSS in to CSS px, 1dppx is equivalent to 96dpi, which corresponds to the default resolution of images displayed in CSS as defined by image-resolution.
/// </summary>
[Literal]
public partial struct dppx : Unit<dppx> { }

/// <summary>
/// Alias for dppx
/// </summary>
[Alias<dppx>]
public partial struct x { };
