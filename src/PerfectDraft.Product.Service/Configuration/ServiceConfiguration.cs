using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Service.Product;

namespace PerfectDraft.Product.Service.Configuration
{
    public static class ServiceConfiguration
    {
        public static void RegisterServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IProductService, ProductService>();
        }
    }
}
