using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{userName}", Name = "GetBasketAsync")]
    public async Task<ActionResult<ShoppingCart>> GetBasketAsync(string userName)
    {
        var basket = await _repository.GetBasketAsync(userName);

        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> UpdateBasketAsync([FromBody] ShoppingCart basket)
    {
        return Ok(await _repository.UpdateBasketAsync(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasketAsync")]
    public async Task<IActionResult> DeleteBasketAsync(string userName)
    {
        await _repository.DeleteBasketAsync(userName);
        return Ok();
    }
}