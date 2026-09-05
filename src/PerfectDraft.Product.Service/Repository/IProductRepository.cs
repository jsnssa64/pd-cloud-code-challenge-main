using PerfectDraft.Product.Shared.Model;

namespace PerfectDraft.Product.Service.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<MagentoProductModel>> GetProducts(CancellationToken cancellationToken);

        Task<IEnumerable<SearchProductModel>> GetSearchProducts(CancellationToken cancellationToken);
    }
}
