using MediatR;
using Ordering.Application.Dtos;

namespace Ordering.Application.Commands;

public record CreateOrderCommand(OrderDto OrderDto) : IRequest
{
}
