using FluentValidation;
using Moq;
using NUnit.Framework;
using PerfectDraft.Product.Api.Controllers;
using PerfectDraft.Product.Service.Product;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Tests.Controller
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController productController;

        public ProductControllerTests() {
            var mockValidator = new Mock<IValidator<ProductSkuDTO>>();
            var mockSearchTermValidator = new Mock<IValidator<ProductSearchTermDTO>>();
            var mockProductService = new Mock<IProductService>();
            productController = new ProductController(
                mockValidator.Object, 
                mockSearchTermValidator.Object, 
                mockProductService.Object);
        }

        [Test]
        public void ProductController_()
        {

        }

    }
}
