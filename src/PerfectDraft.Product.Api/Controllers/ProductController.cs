using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PerfectDraft.Product.Api.DTO;
using PerfectDraft.Product.Service.Product;
using PerfectDraft.Product.Shared.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace PerfectDraft.Product.Api.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductController(
    IValidator<ProductSkuDTO> ProductSkuValidator,  
    IValidator<ProductSearchTermDTO> ProductSearchTermValidator,
    IProductService Service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var Sku = new ProductSkuDTO(id);
        var result = await ProductSkuValidator.ValidateAsync(Sku, cancellationToken);

        if(!result.IsValid)
        {
            return 
                ValidationProblem(
                    new ValidationProblemDetails(
                        result.ToDictionary()
                        )
                    );
        }

        var product = await Service.GetProduct(Sku, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        var response = new ProductResponse(
                new ProductSkuResponse(product.Sku.Sku),
                new ProductMetaDataResponse(product.Name, product.Url),
                new PriceResponse(product.Price, product.Currency),
                product.InStock);
             

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? search, CancellationToken cancellationToken)
    {
        var searchTermDTO = new ProductSearchTermDTO(search);
        var result = await ProductSearchTermValidator.ValidateAsync(searchTermDTO, cancellationToken);

        if (!result.IsValid)
        {
            return
                ValidationProblem(
                    new ValidationProblemDetails(
                        result.ToDictionary()
                        )
                    );
        }

        var product = await Service.SearchProduct(searchTermDTO, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        var response = new ProductResponse(
                new ProductSkuResponse(product.Sku.Sku),
                new ProductMetaDataResponse(product.Name, product.Url),
                new PriceResponse(product.Price, product.Currency),
                product.InStock);

        return Ok(response);
    }

    //return StatusCode(StatusCodes.Status501NotImplemented, new
    //{
    //    message = "Implement product aggregation for GET /products/{id}.",
    //    requestedId = id
    //});
}
