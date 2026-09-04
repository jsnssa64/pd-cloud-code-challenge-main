using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Service.Product;

public interface IProductService
{
    Task GetProduct(ProductSkuDTO sku);
    Task SearchProduct();
}