using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Infrastructure.Repository;

namespace PerfectDraft.Product.Api.Configuration
{
    public static class ServiceConfiguration
    {
        public static void RegisterRepositories(this IServiceCollection Services)
        {
            Services.AddSingleton<IProductRepository, ProductRepository>();
        }
    }
}
