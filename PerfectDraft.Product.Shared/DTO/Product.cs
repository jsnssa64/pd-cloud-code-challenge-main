
namespace PerfectDraft.Product.Shared.DTO;

    public record ProductDTO(
        ProductSkuDTO Sku,
        string Name,
        string Title,
        string Url,
        decimal Price,
        string Currency,
        long Stock,
        bool InStock);