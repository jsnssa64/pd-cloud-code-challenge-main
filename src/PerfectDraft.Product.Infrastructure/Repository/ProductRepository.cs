using Microsoft.Extensions.Options;
using PerfectDraft.Product.Infrastructure.Configuration;
using PerfectDraft.Product.Infrastructure.Infrastructure;
using PerfectDraft.Product.Service.Repository;
using PerfectDraft.Product.Shared.Model;
using System.Text.Json;

namespace PerfectDraft.Product.Infrastructure.Repository
{
    public class ProductRepository(IJsonFileLoader jsonFileLoader, IOptions<DataFileOptions> DataFileOptions) : IProductRepository
    {
        public async Task<IEnumerable<MagentoProductModel>> GetProducts(CancellationToken cancellationToken)
        {
            return await jsonFileLoader.ReadAllJsonFileAsync<MagentoProductModel>(DataFileOptions.Value.MagentoProductsPath, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<SearchProductModel>> GetSearchProducts(CancellationToken cancellationToken)
        {
            try
            {
                return await jsonFileLoader.ReadAllJsonFileAsync<SearchProductModel>(DataFileOptions.Value.SearchProductsPath, cancellationToken: cancellationToken);
            }
            catch (Exception ex) when (ex is JsonException or IOException)
            {
                return Enumerable.Empty<SearchProductModel>();
            }
        }
    }
}
