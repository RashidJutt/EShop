using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Core.Contracts;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Commands.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderService orderService,
        IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }
    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request.OrderDto);
        await _orderService.CreateOrderAsync(orderEntity, cancellationToken);
    }
}
