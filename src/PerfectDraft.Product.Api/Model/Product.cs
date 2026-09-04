
namespace PerfectDraft.Product.Api.DTO;

public record ProductResponse(
    ProductSkuResponse Id,
    ProductMetaDataResponse ProductMetaData,
    PriceResponse Price,
    string Currency,
    string InStock);
