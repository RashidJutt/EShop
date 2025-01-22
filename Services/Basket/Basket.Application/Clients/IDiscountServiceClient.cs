using Discount.Application.Dtos;

namespace Basket.Application.Clients;

public interface IDiscountServiceClient
{
    Task<CouponDto> GetCoupon(string productName);
}
