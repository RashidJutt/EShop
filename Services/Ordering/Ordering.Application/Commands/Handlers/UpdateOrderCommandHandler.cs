using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Contracts;
using Ordering.Core.Repositories;

namespace Ordering.Application.Commands.Handlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateOrderCommandHandler> logger,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteOptimisticUpdateAsync(async () =>
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Order.Id);

            if (orderEntity == null)
            {
                _logger.LogError("Order ({orderId}) not found", request.Order.Id);
                throw new Exception("Order not found");
            }

            var order= _mapper.Map(request.Order, orderEntity);
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync(cancellationToken);
        });
    }
}
