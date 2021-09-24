using System.Threading.Channels;

namespace Radix.Shop.Catalog
{
    public delegate IAsyncEnumerable<T> Search<T>(SearchTerm searchTerm);

    public delegate Search<Product> SearchMerchant(Merchant merchant);

    public delegate Search<T> SearchAll<T>(params Search<T>[] searches);

    public static class Workflows
    {
        public static SearchAll<T> SearchAll<T>() =>
             searches =>
                searchTerm =>
                    SearchAllInternal(searchTerm, searches);

        private static async IAsyncEnumerable<T> SearchAllInternal<T>(SearchTerm searchTerm, Search<T>[] searches)
        {
            var productsChannel = Channel.CreateUnbounded<T>();

            Parallel.ForEach(searches, async search =>
            {
                await foreach (var result in search(searchTerm))
                    await productsChannel.Writer.WriteAsync(result);
            });

            await foreach (var product in productsChannel.Reader.ReadAllAsync())
                   yield return product;
        }
    }
}
