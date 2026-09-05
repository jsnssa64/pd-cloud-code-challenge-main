using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PerfectDraft.Product.Api.Controllers;
using PerfectDraft.Product.Service.Product;
using PerfectDraft.Product.Shared.DTO;

namespace PerfectDraft.Product.Test.Controller
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController productController;
        private Mock<IValidator<ProductSkuDTO>> mockSkuValidator;
        private Mock<IValidator<ProductSearchTermDTO>> mockSearchTermValidator;
        private Mock<IProductService> mockProductService;

        public ProductControllerTests()
        {
            mockSkuValidator = new Mock<IValidator<ProductSkuDTO>>();
            mockSearchTermValidator = new Mock<IValidator<ProductSearchTermDTO>>();
            mockProductService = new Mock<IProductService>();
            productController = new ProductController(
                mockSkuValidator.Object,
                mockSearchTermValidator.Object,
                mockProductService.Object);
        }

        [Test]
        public async Task GetById_FluentValidationFails_Status422WithProblemDetails()
        {
            mockSkuValidator.Setup(sku => sku.ValidateAsync(It.IsAny<ProductSkuDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult([new ValidationFailure("Sku", "Invalid SKU")]));


            var result = await productController.GetById("ValidSKU", new CancellationToken());

            var validationResult = (ObjectResult)result;
            Assert.That(validationResult.StatusCode, Is.EqualTo(422));
            Assert.That(validationResult.Value, Is.InstanceOf<ValidationProblemDetails>());

            var problemDetails = (ValidationProblemDetails)validationResult.Value;
            Assert.That(problemDetails.Errors, Contains.Key("Sku"));
            Assert.That(problemDetails.Errors["Sku"], Does.Contain("Invalid SKU"));
        }

        [Test]
        public async Task GetById_EmptyProduct_ReturnEmptyProducts()
        {
            mockSkuValidator.Setup(sku => sku.ValidateAsync(It.IsAny<ProductSkuDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            mockProductService
                .Setup(productService => productService.GetProduct(It.IsAny<ProductSkuDTO>(), CancellationToken.None))
                .ReturnsAsync((ProductDTO?)null);

            var result = await productController.GetById("ValidSKU", new CancellationToken());

            var validationResult = (NotFoundResult)result;
            Assert.That(validationResult.StatusCode, Is.EqualTo(404));
        }
    }
}