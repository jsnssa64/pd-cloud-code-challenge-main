using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Infrastructure.Configuration;
using PerfectDraft.Product.Infrastructure.Infrastructure;
using PerfectDraft.Product.Infrastructure.Repository;
using System.IO.Abstractions;

namespace PerfectDraft.Product.Api.Configuration
{
    public static class ServiceConfiguration
    {
        public const string DataFilesKey = "DataFiles";

        public static void RegisterRepositories(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.Configure<DataFileOptions>(Configuration.GetSection(DataFilesKey));

            Services.AddSingleton<IFileSystem, FileSystem>();
            Services.AddSingleton<IJsonFileLoader, JsonFileLoader>();
            Services.AddSingleton<IProductRepository, ProductRepository>();
        }
    }
}
