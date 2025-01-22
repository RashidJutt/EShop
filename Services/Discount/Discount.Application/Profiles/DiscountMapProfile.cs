using AutoMapper;
using Discount.Application.Dtos;
using Discount.Core.Entities;

namespace Discount.Application.Profiles;

public class DiscountMapProfile : Profile
{
    public DiscountMapProfile()
    {
        CreateMap<Coupon, CouponDto>().ReverseMap();
    }
}
