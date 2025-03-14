using MediatR;

namespace Ordering.Application.Commands;

public record DeleteOrderCommand(int Id):IRequest
{
}
