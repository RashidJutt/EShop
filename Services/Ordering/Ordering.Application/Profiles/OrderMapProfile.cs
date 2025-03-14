using AutoMapper;
using EventBus.Events;
using Ordering.Application.Dtos;
using Ordering.Core.Entities;

namespace Ordering.Application.Profiles;

public class OrderMapProfile : Profile
{
    public OrderMapProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<Core.Entities.Address, AddressDto>().ReverseMap();
        CreateMap<Core.Entities.PaymentDetails, PaymentDetailsDto>().ReverseMap();
        CreateMap<BasketCheckout, OrderDto>().ForMember(x => x.Id, config => config.Ignore());
        CreateMap<EventBus.Events.Address, AddressDto>();
        CreateMap<EventBus.Events.PaymentDetails,PaymentDetailsDto>();
    }
}
