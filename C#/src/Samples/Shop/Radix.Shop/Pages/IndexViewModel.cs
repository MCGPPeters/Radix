using Microsoft.AspNetCore.Components.Web;
using Radix.Components;

namespace Radix.Shop.Pages
{
    public record IndexViewModel : ViewModel
    {
        public IndexViewModel()
        {
            Types = new List<Type>()
            {
                "Mug",
                "Sheet",
                "T-Shirt"
            };

            Brands = new List<Brand>()
            {
                ".NET",
                "Azure",
                "Other"
            };
        }
        public string TypeFilter { get; internal set; }
        public string BrandFilter { get; internal set; }
        public IEnumerable<Product> FilteredProducts { get; internal set; }
        public IEnumerable<Type> Types { get; internal set; }
        public IEnumerable<Brand> Brands { get; internal set; } 

        internal Action<MouseEventArgs> ApplyFilter() => _ => { };
    }
}
