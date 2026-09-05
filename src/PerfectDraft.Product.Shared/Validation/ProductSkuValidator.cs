using FluentValidation;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Shared.Validation
{
    public static class SkuErrorMessage
    {
        public const string Empty = "Please specify Product SKU";
        public const string Length = "Product SKU Invalid Length";
        public const string Invalid = "Invalid Product SKU";


    }
    public class ProductSkuValidator : AbstractValidator<ProductSkuDTO>
    {
        public static readonly string[] ProductPrefix = new[] { "P", "M" };
        public ProductSkuValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty()
                .WithMessage(SkuErrorMessage.Empty);

            RuleFor(x => x.Sku)
                .Length(2, 5)
                .WithMessage(SkuErrorMessage.Length);

            RuleFor(x => x.Sku)
                .Must(value => ProductPrefix.Any(p => value.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                .WithMessage(SkuErrorMessage.Invalid);
        }
    }
}
