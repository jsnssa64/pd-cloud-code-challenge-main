using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using PerfectDraft.Product.Shared.DTO;
using PerfectDraft.Product.Service.Product;

namespace PerfectDraft.Product.Api.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductController(IValidator<ProductSkuDTO> ProductSkuValidator,  IProductService Service) : ControllerBase
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

        await Service.GetProduct(Sku);
                
        //return StatusCode(StatusCodes.Status501NotImplemented, new
        //{
        //    message = "Implement product aggregation for GET /products/{id}.",
        //    requestedId = id
        //});

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? search, CancellationToken cancellationToken)
    {
        await Service.SearchProduct();  
        await Task.CompletedTask;
        return StatusCode(StatusCodes.Status501NotImplemented, new
        {
            message = "Implement product aggregation for GET /products?search=.",
            search
        });
    }
}
