using FluentValidation;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Shared.Validation
{
    public class ProductSkuValidator : AbstractValidator<ProductSkuDTO>
    {
        public const string ProductPrefix = "P";
        public ProductSkuValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty()
                .WithMessage("Invalid Product SKU");
            RuleFor(x => x.Sku)
                .Length(2, 5)
                .WithMessage("Invalid Product SKU");

            RuleFor(x => x.Sku)
                .Must(value => value.StartsWith(ProductPrefix))
                .WithMessage("Invalid Product SKU");
        }
    }
}
