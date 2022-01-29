namespace Radix.Web.Css.Data.Units.Length.Relative;

/// <summary>
/// Font size of the parent, in the case of typographical properties like font-size, and font size of the element itself, in the case of other properties like width.
/// </summary>
[Literal]
public partial struct em : Unit { }

/// <summary>
/// x-height of the element's font.
/// </summary>
[Literal]
public partial struct ex : Unit { }

/// <summary>
/// The advance measure (width) of the glyph "0" of the element's font.
/// </summary>
[Literal]
public partial struct ch : Unit { }

/// <summary>
/// Font size of the root element.
/// </summary>
[Literal]
public partial struct rem : Unit { }

/// <summary>
/// Line height of the element.
/// </summary>
[Literal]
public partial struct lh : Unit { }

/// <summary>
/// 1% of the viewport's width.
/// </summary>
[Literal]
public partial struct vw : Unit { }

/// <summary>
/// 1% of the viewport's height.
/// </summary>
[Literal]
public partial struct vh : Unit { }

/// <summary>
/// 1% of the viewport's smaller dimension.
/// </summary>
[Literal]
public partial struct vmin : Unit { }

/// <summary>
/// 1% of the viewport's larger dimension.
/// </summary>
[Literal]
public partial struct vmax : Unit { }
