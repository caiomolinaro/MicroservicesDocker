using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
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
        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName!);
            item.Price -= coupon.Amount;
        }

        return Ok(await _repository.UpdateBasketAsync(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasketAsync")]
    public async Task<IActionResult> DeleteBasketAsync(string userName)
    {
        await _repository.DeleteBasketAsync(userName);
        return Ok();
    }
}