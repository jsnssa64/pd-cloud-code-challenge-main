using FluentValidation.TestHelper;
using PerfectDraft.Product.Shared.DTO;
using PerfectDraft.Product.Shared.Validation;

namespace PerfectDraft.Product.Test.Validator
{
    [TestFixture]
    public class ProductValidatorTests
    {
        private readonly List<string> ErrorMessages = new()
        {
            "Please specify Search Terms",
            "Invalid Search Term Length"
        };

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
            var messages = result.Errors
              .Select(e => e.ErrorMessage);

            Assert.That(messages, Is.EquivalentTo(ErrorMessages));
        }
    }
}
