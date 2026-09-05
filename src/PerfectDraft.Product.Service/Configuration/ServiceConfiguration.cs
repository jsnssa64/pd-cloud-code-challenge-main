using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Infrastructure.Configuration;
using PerfectDraft.Product.Service.Product;

namespace PerfectDraft.Product.Service.Configuration
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.RegisterRepositories(configuration);
            Services.AddSingleton<IProductService, ProductService>();
        }
    }
}
