using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Shop.Catalog.Interface.Logic.Components
{
    public record ProductModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string ImageSource { get; set; }

        public string MerchantName { get; set; }
        public string PriceUnits { get; set; }
        public string PriceFraction { get; set; }
        public string UnitSize { get; set; }

        public string UnitOfMeasure { get; set; }

        public string PricePerUnitPriceUnits { get; set; }
        public string PricePerUnitPriceFraction { get; set; }
    }
}
