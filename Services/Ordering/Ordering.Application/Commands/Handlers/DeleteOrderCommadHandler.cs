using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Contracts;
using Ordering.Core.Repositories;

namespace Ordering.Application.Commands.Handlers;

public class DeleteOrderCommadHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteOrderCommadHandler> _logger;

    public DeleteOrderCommadHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteOrderCommadHandler> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteOptimisticUpdateAsync(async () =>
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                _logger.LogError("Order ({orderId}) not found", request.Id);
                throw new Exception("Order not found");
            }

            await _orderRepository.DeleteAsync(order);
            await _unitOfWork.CommitAsync(cancellationToken);
        });
    }
}
