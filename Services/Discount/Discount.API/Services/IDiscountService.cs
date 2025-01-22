using Discount.Application.Dtos;
using System.ServiceModel;

namespace Discount.API.Services;

[ServiceContract]
public interface IDiscountService
{
    [OperationContract]
    Task<CouponDto?> GetDiscount(CouponRequestDto couponRequest);

    [OperationContract]
    Task<CouponDto> CreateDiscount(CouponDto coupon);

    [OperationContract]
    Task<CouponDto> UpdateDiscount(CouponDto coupon);

    [OperationContract]
    Task<DeleteResultDto> DeleteDiscount(CouponRequestDto deleteRequst);
}
