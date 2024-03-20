using AutoMapper;
using Discount.API.Repositories;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscountAsync(request.ProductName);

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with ProductName = {request.ProductName} not found"));
        }

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.CreateDiscountAsync(coupon);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        await _repository.UpdateDiscountAsync(coupon);

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscountAsync(request.ProductName);

        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };
        return response;
    }
}