using PerfectDraft.Product.Infrastructure.Model;

namespace PerfectDraft.Product.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<MagentoProductModel>> GetProducts(CancellationToken cancellationToken);

        Task<IEnumerable<SearchProductModel>> GetSearchProducts(CancellationToken cancellationToken);
    }
}
