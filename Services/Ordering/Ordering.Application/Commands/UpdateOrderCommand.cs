using MediatR;
using Ordering.Application.Dtos;

namespace Ordering.Application.Commands;

public record UpdateOrderCommand(OrderDto Order):IRequest
{
}
