using MediatR;
using Ordering.Application.Dtos;

namespace Ordering.Application.Queries;

public record GetOrdersQuery(string userName) : IRequest<List<OrderDto>>
{
}
