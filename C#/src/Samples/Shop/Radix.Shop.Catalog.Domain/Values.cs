using Radix.Data;
using Radix.Data.String.Validity;

namespace Radix.Shop.Catalog.Domain;

[Validated<string, IsNotNullOrEmpty>]
public partial record Id { }

[Validated<string, IsNotNullOrEmpty>]
public partial record ProductTitle { }

[Alias<string>]
public partial record ProductDescription { }

[Validated<string, IsNotNullOrEmpty>]
public partial record ProductImageSource { }

[Validated<string, IsNotNullOrEmpty>]
public partial record MerchantName { }

[Validated<string, IsNotNullOrEmpty>]
public partial record MerchantSearchUri { }

[Parsed<int, Data.Int.Read>]
public partial record PriceUnits { }

[Parsed<int, Data.Int.Read>]
public partial record PriceFraction { }

[Alias<string>]
public partial struct SearchTerm { }

[Parsed<decimal, Data.Decimal.Read>]
public partial record UnitSize { }

[Validated<string, IsNotNullOrEmpty>]
public partial record UnitOfMeasure { }
