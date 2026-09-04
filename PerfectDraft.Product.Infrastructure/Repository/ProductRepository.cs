using PerfectDraft.Product.Infrastructure.Infrastructure;
using PerfectDraft.Product.Infrastructure.Model;

namespace PerfectDraft.Product.Infrastructure.Repository
{
    public class ProductRepository(IJsonFileLoader jsonFileLoader) : IProductRepository
    {
        public const string BasePath = "./Data/";
        public const string ProductFileName = "magento-products.json";
        public const string SearchFileName = "search-products.json";

        public async Task<IEnumerable<MagentoProductModel>> GetProducts(CancellationToken cancellationToken)
        {
            return await jsonFileLoader.ReadAllJsonFileAsync<MagentoProductModel>(BasePath + ProductFileName, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<SearchProductModel>> GetSearchProducts(CancellationToken cancellationToken)
        {
            return await jsonFileLoader.ReadAllJsonFileAsync<SearchProductModel>(BasePath + ProductFileName, cancellationToken: cancellationToken);
        }
    }
}
