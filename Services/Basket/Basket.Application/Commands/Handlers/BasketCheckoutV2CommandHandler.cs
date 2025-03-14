using AutoMapper;
using Basket.Core.Repositories;
using EventBus.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Commands.Handlers;

public class BasketCheckoutV2CommandHandler : IRequestHandler<BasketCheckoutV2Command>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketCheckoutCommandHandler> _logger;
    private readonly IMapper _mapper;

    public BasketCheckoutV2CommandHandler(
        IPublishEndpoint publishEndpoint, 
        IBasketRepository basketRepository, 
        ILogger<BasketCheckoutCommandHandler> logger,
        IMapper mapper)
    {
        _publishEndpoint = publishEndpoint;
        _basketRepository = basketRepository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task Handle(BasketCheckoutV2Command request, CancellationToken cancellationToken)
    {
        var eventMessage = _mapper.Map<BasketCheckoutV2>(request.BasketCheckoutDto);
        await _publishEndpoint.Publish(eventMessage, cancellationToken).ConfigureAwait(false);
        await _basketRepository.DeleteAsync(request.BasketCheckoutDto.UserName, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Basket published for {userName}",request.BasketCheckoutDto.UserName);
    }
}
