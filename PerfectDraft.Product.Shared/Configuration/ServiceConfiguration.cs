using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PerfectDraft.Product.Shared.Validation;

namespace PerfectDraft.Product.Shared.ValidatorConfiguration
{
    public static class ServiceSharedConfiguration
    {
        public static void RegisterDTOValidators(this IServiceCollection Services)
        {
            Services.AddValidatorsFromAssemblyContaining(typeof(ProductSkuValidator).Assembly.GetType());
        }
    }
}
