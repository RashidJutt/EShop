using AutoMapper;
using Basket.Application.Dtos;
using Basket.Core.Entities;

namespace Basket.Application.Profiles;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
        CreateMap<ShoppingCartItemDto, ShoppingCartItem>().ReverseMap();
    }
}
