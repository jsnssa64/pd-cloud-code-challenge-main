using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetById(string? id, CancellationToken cancellationToken)
    {
        if(id is  null)
            return BadRequest();

        var Sku = new ProductSkuDTO(id);
        var result = await ProductSkuValidator.ValidateAsync(Sku, cancellationToken);

        if(!result.IsValid)
        {
            return 
                UnprocessableEntity(
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

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? search, CancellationToken cancellationToken)
    {
        if(search is null)
            return BadRequest();

        var searchTermDTO = new ProductSearchTermDTO(search);
        var result = await ProductSearchTermValidator.ValidateAsync(searchTermDTO, cancellationToken);

        if (!result.IsValid)
        {
            return
                UnprocessableEntity(
                    new ValidationProblemDetails(
                        result.ToDictionary()
                        )
                    );
        }

        var product = await Service.SearchProduct(searchTermDTO, cancellationToken);

        return Ok(product);
    }
}
