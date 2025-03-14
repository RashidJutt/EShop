using EventBus.Events;
using MassTransit;
using Ordering.Application.Services;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.API.Consumers;

public class BasketCheckoutV2Consumer : IConsumer<BasketCheckoutV2>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<BasketCheckoutV2Consumer> _logger;

    public BasketCheckoutV2Consumer(IOrderService orderService, ILogger<BasketCheckoutV2Consumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutV2> context)
    {
        var message = context.Message;
        var order = new Order
        {
            UserName = message.UserName,
            TotalPrice = message.TotalPrice,
            EmailAddress = message.EmailAddress,
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = new Core.Entities.Address
            {
                Country = string.Empty,
                Line = string.Empty,
                State = string.Empty,
                ZipCode = string.Empty
            },
            PaymentDetails = new Core.Entities.PaymentDetails
            {
                CardName = string.Empty,
                CardNumber = string.Empty,
                Cvv = string.Empty,
                Expiration = string.Empty,
                PaymentMethod = 1,
            }
        };

        await _orderService.CreateOrderAsync(order, context.CancellationToken);
        _logger.LogInformation("BasketCheckoutV2Event consumed successfully. Created Order Id : {OrderId}", order.Id);
    }
}

