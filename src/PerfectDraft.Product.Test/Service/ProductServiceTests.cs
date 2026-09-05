using AutoFixture;
using Moq;
using PerfectDraft.Product.Service.Product;
using PerfectDraft.Product.Service.Repository;
using PerfectDraft.Product.Shared.DTO;
using PerfectDraft.Product.Shared.Model;

namespace PerfectDraft.Product.Test.Service
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> productRepository;
        private Fixture fixture;
        private ProductService productService;

        public ProductServiceTests()
        {
            productRepository = new Mock<IProductRepository>();
            fixture = new Fixture();

            productService = new ProductService(productRepository.Object);
        }

        [Test]
        public async Task GetProduct_ValidMagentaWithInvalidSearchProduct_Successful()
        {
            var Sku = fixture.Create<ProductSkuDTO>();
            var magentoProductList = fixture.
                CreateMany<MagentoProductModel>().
                ToList();

            var product = fixture.
                Build<MagentoProductModel>().
                With(x => x.Sku, Sku.Sku)
                .Create();

            magentoProductList.Add(product);

            productRepository.Setup(repo => repo.GetProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(magentoProductList);


            productRepository.Setup(repo => repo.GetSearchProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<SearchProductModel>());

            var result = await productService.GetProduct(Sku, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Sku.Sku, Is.EqualTo(Sku.Sku));
        }


        [Test]
        public async Task GetSearchProducts_ValidSearchTerm_ValidProductMagentoOverride()
        {
            var searchTermDTO = new ProductSearchTermDTO("RandomValue12345");

            var magentoProductModel = fixture.Build<MagentoProductModel>()
                .With(x => x.Name, "Test " + searchTermDTO.Search + " Random Test")
                .Create();

            var searchProductModel = fixture.Build<SearchProductModel>()
                .With(spm => spm.Sku, magentoProductModel.Sku)
                .Create();

            var searchProductList = fixture.
                CreateMany<SearchProductModel>().
                ToList();
            searchProductList.Add(searchProductModel);

            var magentoProductList = fixture.
                CreateMany<MagentoProductModel>().
                ToList();
            magentoProductList.Add(magentoProductModel);

            productRepository.Setup(repo => repo.GetProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(magentoProductList);

            productRepository.Setup(repo => repo.GetSearchProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchProductList);

            var result = await productService.SearchProduct(searchTermDTO, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, 
                Has.One.Matches<ProductDTO>(p =>
                    p.Sku.Sku == magentoProductModel.Sku &&
                    p.Name == magentoProductModel.Name &&
                    p.Title == searchProductModel.Title &&
                    p.Url == searchProductModel.Image &&
                    p.InStock == searchProductModel.InStock &&
                    p.Stock == magentoProductModel.Stock &&
                    p.Currency == magentoProductModel.Currency &&
                    p.Price == magentoProductModel.Price));
        }

        [Test]
        public async Task GetSearchProducts_WrongSearchTerm_EmptyProductList()
        {
            var searchTermDTO = new ProductSearchTermDTO("UNKNOWN");

            var searchProductList = fixture.
                CreateMany<SearchProductModel>().
                ToList();

            var magentoProductList = fixture.
                CreateMany<MagentoProductModel>().
                ToList();

            productRepository.Setup(repo => repo.GetProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(magentoProductList);

            productRepository.Setup(repo => repo.GetSearchProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchProductList);

            var result = await productService.SearchProduct(searchTermDTO, CancellationToken.None);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetProduct_MissingEntry_EmptyProducts()
        {
            var Sku = fixture.Create<ProductSkuDTO>();

            var magentoProductList = fixture.
                CreateMany<MagentoProductModel>().
                ToList();

            productRepository.Setup(repo => repo.GetProducts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(magentoProductList);

            var result = await productService.GetProduct(Sku, new CancellationToken());

            Assert.That(result, Is.Null);
        }
    }
}
