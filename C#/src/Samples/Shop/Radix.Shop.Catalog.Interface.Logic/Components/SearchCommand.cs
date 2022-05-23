namespace Radix.Shop.Catalog.Interface.Logic.Components
{
    public class SearchCommand
    {
        public SearchCommand(string searchTerm) => SearchTerm = searchTerm;

        public string SearchTerm { get; }
    }
}