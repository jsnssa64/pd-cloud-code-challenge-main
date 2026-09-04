using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Service.Product;

public interface IProductService
{
    Task<ProductDTO?> GetProduct(ProductSkuDTO sku, CancellationToken cancellationToken);
    Task<ProductDTO?> SearchProduct(ProductSearchTermDTO searchTerm, CancellationToken cancellationToken);
}