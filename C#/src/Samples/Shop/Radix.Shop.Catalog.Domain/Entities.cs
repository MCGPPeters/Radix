using System.Globalization;

namespace Radix.Shop.Catalog.Domain;


public record Product
{
    private Product(Id Id, ProductTitle title, ProductDescription description, MerchantName merchantName, Price price, ProductImageSource imageSource, UnitSize unitSize, UnitOfMeasure unitOfMeasure)
    {
        this.Id = Id;
        Title = title;
        Description = description;
        MerchantName = merchantName;
        Price = price;
        ImageSource = imageSource;
        UnitSize = unitSize;
        UnitOfMeasure = unitOfMeasure;
        decimal pricePerUnit = decimal.Parse($"{price.Units}{NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}{price.Fraction}", CultureInfo.CurrentCulture) / unitSize;
        var rounded = System.Math.Round(pricePerUnit, 2);
        var unitPrice = rounded.ToString();
        var priceParts = unitPrice.Split(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
        PricePerUnit = new(priceParts[0], priceParts[1]);
    }

    private static Func<Id, ProductTitle, ProductDescription, MerchantName, Price, ProductImageSource, UnitSize, UnitOfMeasure, Product> New => (id, title, description, merchantName, price, imageSource, unitSize, unitOfMeasure) =>
        new Product(id, title, description, merchantName, price, imageSource, unitSize, unitOfMeasure);

    public static Validated<Product> Create(string id, string title, string description, string merchantName, string priceUnits, string priceFraction, string imageSource, string unitSize, string unitOfMeasure) =>
        Valid(New)
        .Apply(Id.Create(id))
        .Apply(ProductTitle.Create(title))
        .Apply(ProductDescription.Create(description))
        .Apply(MerchantName.Create(merchantName))
        .Apply(Price.Create(priceUnits, priceFraction))
        .Apply(ProductImageSource.Create(imageSource))
        .Apply(UnitSize.Create(unitSize))
        .Apply(UnitOfMeasure.Create(unitOfMeasure));

    public Id Id {  get; }
    public ProductTitle Title { get; }
    public ProductDescription Description { get;  }
    public ProductImageSource ImageSource { get;  }
    public MerchantName MerchantName { get; }
    public Price Price { get; }
    public UnitSize UnitSize { get;  }

    public UnitOfMeasure UnitOfMeasure { get; }
    public PricePerUnit PricePerUnit { get; }
}

public record Price
{
    private Price(PriceUnits units, PriceFraction fraction)
    {
        Units = units;
        Fraction = fraction;
    }

    private static Func<PriceUnits, PriceFraction, Price> New => (units, fraction) => new Price(units, fraction);

    public static Validated<Price> Create(string units, string fraction) =>
        Valid(New)
        .Apply(PriceUnits.Create(units))
        .Apply(PriceFraction.Create(fraction));

    public PriceUnits Units { get; }
    public PriceFraction Fraction { get; }
}

public record PricePerUnit(string Units, string Fraction);

