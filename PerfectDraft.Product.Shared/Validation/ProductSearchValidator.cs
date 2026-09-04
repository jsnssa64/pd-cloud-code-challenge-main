using FluentValidation;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Shared.Validation
{
    public static class SearchTermErrorMessage
    {
        public const string Empty = "Please specify Search Terms";
        public const string Length = "Invalid Search Term Length";
    }

    public class ProductSearchValidator : AbstractValidator<ProductSearchTermDTO>
    {
        public ProductSearchValidator()
        {
            RuleFor(x => x.Search)
                .NotEmpty()
                .WithMessage(SearchTermErrorMessage.Empty);
            RuleFor(x => x.Search)
                .Length(1, 250)
                .WithMessage(SearchTermErrorMessage.Length);
        }
    }
}
