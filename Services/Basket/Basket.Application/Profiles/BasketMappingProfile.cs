using AutoMapper;
using Basket.Application.Dtos;
using Basket.Core.Entities;
using EventBus.Events;

namespace Basket.Application.Profiles;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
        CreateMap<ShoppingCartItemDto, ShoppingCartItem>().ReverseMap();
        CreateMap<AddressDto, Address>().ReverseMap();
        CreateMap<PaymentDetailsDto, PaymentDetails>().ReverseMap();
        CreateMap<BasketCheckoutDto, EventBus.Events.BasketCheckout>()
            .ForMember(x => x.CorrelationId, config => config.Ignore())
            .ForMember(x => x.CreationDate, config => config.Ignore());

        CreateMap<BasketCheckoutV2Dto, BasketCheckoutV2>()
            .ForMember(x => x.CorrelationId, config => config.Ignore())
            .ForMember(x => x.CreationDate, config => config.Ignore());
    }
}
