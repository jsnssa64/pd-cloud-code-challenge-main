
namespace PerfectDraft.Product.Shared.DTO;

    public record ProductDTO(
        ProductSkuDTO Sku, 
        string Name, 
        string Url,
        decimal Price,
        string Currency,
        string InStock);