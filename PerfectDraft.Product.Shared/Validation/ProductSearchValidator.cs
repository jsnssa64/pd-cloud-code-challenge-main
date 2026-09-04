using FluentValidation;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Shared.Validation
{
    public class ProductSearchValidator : AbstractValidator<ProductSearchTermDTO>
    {
        public ProductSearchValidator()
        {
            RuleFor(x => x.Search)
                .NotEmpty()
                .WithMessage("Please specify Search Terms");
            RuleFor(x => x.Search)
                .Length(1, 250)
                .WithMessage("Search Term has reached the Maximum Length 250 or Minimum 1");
        }
    }
}
