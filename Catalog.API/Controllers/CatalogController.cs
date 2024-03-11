using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _repository;

    public CatalogController(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
    {
        var products = await _repository.GetProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProductByIdAsync")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<Product>> GetProductByIdAsync(string id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryAsync(string category)
    {
        if (category is null)
        {
            return BadRequest("Invalid category");
        }

        var products = await _repository.GetProductByCategoryAsync(category);

        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateProductAsync([FromBody] Product product)
    {
        if (product is null)
        {
            return BadRequest("Invalid Product");
        }

        await _repository.CreateProductAsync(product);

        return CreatedAtRoute("GetProductByIdAsync", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProductAsaync([FromBody] Product product)
    {
        if (product is null)
        {
            return BadRequest("Invalid Product");
        }

        return Ok(await _repository.UpdateProductAsync(product));
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProductAsync")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProductByIdAsync(string id)
    {
        return Ok(await _repository.DeleteProductAsync(id));
    }
}