
namespace PerfectDraft.Product.Shared.DTO;

    public record ProductDTO(
        ProductSkuDTO Sku, 
        string Name, 
        decimal Price,
        string Currency,
        string Stock);