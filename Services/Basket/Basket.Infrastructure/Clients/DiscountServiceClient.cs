using Basket.Application.Clients;
using Discount.API.Services;

namespace Basket.Infrastructure.Clients;
public class DiscountServiceClient : IDiscountServiceClient
{
    private readonly DiscountService.DiscountServiceClient _discountServiceClient;

    public DiscountServiceClient(
        DiscountService.DiscountServiceClient discountServiceClient)
    {
        _discountServiceClient = discountServiceClient;
    }

    public async Task<Discount.Application.Dtos.CouponDto> GetCoupon(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
        }

        var couponResponse = await _discountServiceClient.GetDiscountAsync(new CouponRequestDto { ProductName = productName });
        return MapCoupon(couponResponse);
    }

    private Discount.Application.Dtos.CouponDto MapCoupon(Discount.API.Services.CouponDto couponResponse)
    {
        return new Discount.Application.Dtos.CouponDto
        {
            Id = couponResponse.Id,
            Amount = decimal.Parse(couponResponse.Amount),
            ProductName = couponResponse.ProductName,
            Description = couponResponse.Description,
        };
    }
}
