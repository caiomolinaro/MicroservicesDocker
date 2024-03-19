using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        _repository = discountRepository;
    }

    [HttpGet("{productName}", Name = "GetDiscountAsync")]
    public async Task<ActionResult<Coupon>> GetDiscountAsync(string productName)
    {
        var coupon = await _repository.GetDiscountAsync(productName);
        return Ok(coupon);
    }

    [HttpPost]
    public async Task<ActionResult<Coupon>> CreateDiscountAsync([FromBody] Coupon coupon)
    {
        await _repository.CreateDiscountAsync(coupon);
        return CreatedAtRoute("GetDiscountAsync", new { productName = coupon.ProductName }, coupon);
    }

    [HttpPut]
    public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
    {
        return Ok(await _repository.UpdateDiscountAsync(coupon));
    }

    [HttpDelete("{productName}", Name = "DeleteDiscountAsync")]
    public async Task<ActionResult<bool>> DeleteDiscountAsync(string productName)
    {
        return Ok(await _repository.DeleteDiscountAsync(productName));
    }
}