using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Contracts;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.Threading;

namespace Ordering.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteOptimisticUpdateAsync(async () =>
        {
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Order is successfully created for {userName}", order.UserName);
        });
    }
}
