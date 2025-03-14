using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Core.Repositories;

namespace Ordering.Application.Queries.Handlers;

public class GetOrderQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrderQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(
        IOrderRepository orderRepository,
        ILogger<GetOrderQueryHandler> logger,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retriving orders for user {userName}", request.userName);
        var orders = await _orderRepository.GetAllAsync(x => x.UserName.Contains(request.userName));
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
