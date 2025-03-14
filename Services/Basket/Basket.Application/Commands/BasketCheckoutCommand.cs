using Basket.Application.Dtos;
using MediatR;

namespace Basket.Application.Commands;

public record BasketCheckoutCommand(BasketCheckoutDto BasketCheckoutDto):IRequest
{
}
