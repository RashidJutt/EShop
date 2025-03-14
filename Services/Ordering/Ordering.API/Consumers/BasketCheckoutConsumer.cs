using AutoMapper;
using EventBus.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Dtos;

namespace Ordering.API.Consumers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckout>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(
        IMapper mapper,
        IMediator mediator,
        ILogger<BasketCheckoutConsumer> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<BasketCheckout> context)
    {
        using var scope = _logger.BeginScope("Consuming basket checkout event for correlation id {correlationId}", context.Message.CorrelationId);
        var orderDto = _mapper.Map<OrderDto>(context.Message);
        await _mediator.Send(new CreateOrderCommand(orderDto));
        _logger.LogInformation("Order Created for user {userName}", orderDto.UserName);
    }
}
