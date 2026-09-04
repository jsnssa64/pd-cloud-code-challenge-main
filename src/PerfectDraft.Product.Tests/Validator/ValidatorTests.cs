using FluentValidation.TestHelper;
using NUnit.Framework;
using PerfectDraft.Product.Shared.DTO;
using PerfectDraft.Product.Shared.Validation;

namespace PerfectDraft.Product.Tests.Validator
{
    public class ProductValidatorTests
    {
        private ProductSearchValidator _validator;

        public ProductValidatorTests()
        {
            _validator = new ProductSearchValidator();
        }

        [TestCase("")]
        public void Test(string SearchTerm)
        {
            //  
            var searchTermDTO = new ProductSearchTermDTO(SearchTerm);

            //  Action
            var result = _validator.TestValidate(searchTermDTO);

            // Assert

            result.ShouldHaveValidationErrorFor(c => c.Search);
            result.ShouldNotHaveValidationErrorFor(c => c.Search);
        }
    }
}
