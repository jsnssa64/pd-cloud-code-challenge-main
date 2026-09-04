using Microsoft.Extensions.Logging;
using PerfectDraft.Product.Infrastructure.Repository;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Service.Product;
public class ProductService(ILogger<ProductService> Logger, IProductRepository ProductRepository) : IProductService
{
    public async Task<ProductDTO?> GetProduct(ProductSkuDTO Sku, CancellationToken cancellationToken)
    {
        try
        {
            var products = await ProductRepository.GetProducts(cancellationToken);
            var searchProducts = await ProductRepository.GetSearchProducts(cancellationToken);

        }
        catch (Exception ex)
        {
            //  Do not Block just Return empty object
            Logger.LogError(ex, "");
        }

        return null;
    }

    public async Task<ProductDTO?> SearchProduct(ProductSearchTermDTO searchTerm, CancellationToken cancellationToken)
    {
        try
        {
            var products = await ProductRepository.GetProducts(cancellationToken);
            var searchProducts = await ProductRepository.GetSearchProducts(cancellationToken);

        }
        catch (Exception ex)
        {
            //  Do not Block just Return empty object
            Logger.LogError(ex, "");
        }

        return null;
    }
}
