using PerfectDraft.Product.Infrastructure.Repository;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Service.Product;
public class ProductService(IProductRepository ProductRepository) : IProductService
{
    public async Task<ProductDTO?> GetProduct(ProductSkuDTO Sku, CancellationToken cancellationToken)
    {
        var getProducts = ProductRepository.GetProducts(cancellationToken);
        var searchProducts = ProductRepository.GetSearchProducts(cancellationToken);

        await Task.WhenAll(getProducts, searchProducts);

        var product = getProducts.Result.FirstOrDefault(product => string.Equals(product.Sku, Sku.Sku, StringComparison.OrdinalIgnoreCase));

        if (product is null)
        {
            return null;
        }

        var productMetaData = searchProducts.Result.FirstOrDefault(sproduct => string.Equals(sproduct.Sku, product.Sku, StringComparison.Ordinal));

        return new ProductDTO(
            new ProductSkuDTO(product.Sku),
            product.Name,
            productMetaData?.Title ?? product.Name,
            productMetaData?.Image ?? "http://default.com/default.png",
            product.Price,
            product.Currency,
            product.Stock,
            (product.Stock > 0)
            );
    }

    public async Task<IEnumerable<ProductDTO>> SearchProduct(ProductSearchTermDTO searchTerm, CancellationToken cancellationToken)
    {
     
        var getProducts = ProductRepository.GetProducts(cancellationToken);
        var searchProducts = ProductRepository.GetSearchProducts(cancellationToken);

        await Task.WhenAll(getProducts, searchProducts);


        var searchTerms = searchTerm.Search.Split(' ', StringSplitOptions.TrimEntries);

        var filteredProducts = getProducts.Result
            .Where(product => searchTerms.Any(term => product.Name.Contains(term, StringComparison.OrdinalIgnoreCase)));

        var searchProductsResults = await searchProducts;

        return filteredProducts.Select(product =>
        {
            var productMetaData = searchProductsResults.FirstOrDefault(sproduct => string.Equals(sproduct.Sku, product.Sku, StringComparison.Ordinal));
            
            return new ProductDTO(
                new ProductSkuDTO(product.Sku),
                product.Name,
                productMetaData?.Title ?? product.Name,
                //  Default to a default Image
                productMetaData?.Image ?? "http://default.com/default.png",
                product.Price,
                product.Currency,
                product.Stock,
                (product.Stock > 0)
            );
        });
    }
}
