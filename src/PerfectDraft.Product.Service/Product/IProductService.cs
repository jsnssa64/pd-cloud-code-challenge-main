using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Service.Product;

public interface IProductService
{
    Task<ProductDTO?> GetProductAsync(ProductSkuDTO sku, CancellationToken cancellationToken);
    Task<IEnumerable<ProductDTO>> SearchProductAsync(ProductSearchTermDTO searchTerm, CancellationToken cancellationToken);
}