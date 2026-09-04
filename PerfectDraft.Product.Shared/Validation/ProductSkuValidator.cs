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
        public const string ProductPrefix = "P";
        public ProductSkuValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty()
                .WithMessage(SkuErrorMessage.Empty);

            RuleFor(x => x.Sku)
                .Length(2, 5)
                .WithMessage(SkuErrorMessage.Length);

            RuleFor(x => x.Sku)
                .Must(value => value.StartsWith(ProductPrefix))
                .WithMessage(SkuErrorMessage.Invalid);
        }
    }
}
