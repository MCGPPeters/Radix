﻿using Radix.Data;
using Radix.Data.String.Validity;

namespace Radix.Shop.Catalog.Domain;

[Validated<string, IsNotNullOrEmpty>]
public partial record Id { }

[Validated<string, IsNotNullOrEmpty>]
public partial record ProductTitle { }

[Validated<string, IsNotNullOrEmpty>]
public partial record ProductDescription { }

[Validated<string, IsNotNullOrEmpty>]
public partial record ProductImageSource { }

[Validated<string, IsNotNullOrEmpty>]
public partial record MerchantName { }

[Validated<string, IsNotNullOrEmpty>]
public partial record MerchantSearchUri { }

[Parsed<int, Data.Int.FromString>]
public partial record PriceUnits { }

[Parsed<int, Data.Int.FromString>]
public partial record PriceFraction { }

[Alias<string>]
public partial struct SearchTerm { }

[Parsed<int, Data.Int.FromString>]
public partial record UnitSize { }

[Validated<string, IsNotNullOrEmpty>]
public partial record UnitOfMeasure { }
