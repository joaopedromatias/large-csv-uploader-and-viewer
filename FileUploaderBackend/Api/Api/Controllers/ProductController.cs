using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("Search")]

    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] DateOnly? expiration,
        [FromQuery] uint page,
        [FromQuery] uint pageSize,
        [FromQuery] string orderKey,
        [FromQuery] bool orderDesc,
        CancellationToken cancellationToken
        )
    {
        var productQuery = new ProductQueryDto() 
        {
            Name = name,
            Expiration = expiration,
            Page = page,
            PageSize = pageSize,
            OrderKey = orderKey,
            OrderDesc = orderDesc
        };

        var (isQueryValid, message) = productQuery.IsValid();
        if (!isQueryValid)
            return BadRequest($"Invalid query: {message}");

        var products = await _productService.GetProductsWithExchange(productQuery, cancellationToken);

        return Ok(new { products });
    }
}
