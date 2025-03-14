using Basket.Application.Dtos;
using MediatR;

namespace Basket.Application.Commands;

public record BasketCheckoutV2Command(BasketCheckoutV2Dto BasketCheckoutDto) : IRequest
{
}
