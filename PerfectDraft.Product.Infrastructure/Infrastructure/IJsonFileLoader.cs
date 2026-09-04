namespace PerfectDraft.Product.Infrastructure.Infrastructure
{
    public interface IJsonFileLoader
    {
        Task<IEnumerable<T>> ReadAllJsonFileAsync<T>(string path, CancellationToken cancellationToken);
    }
}
