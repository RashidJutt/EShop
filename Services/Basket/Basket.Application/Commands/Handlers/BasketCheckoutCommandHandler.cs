using AutoMapper;
using Basket.Core.Repositories;
using EventBus.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Commands.Handlers;

public class BasketCheckoutCommandHandler : IRequestHandler<BasketCheckoutCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketCheckoutCommandHandler> _logger;
    private readonly IMapper _mapper;

    public BasketCheckoutCommandHandler(
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
    public async Task Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
    {
        var eventMessage = _mapper.Map<BasketCheckout>(request.BasketCheckoutDto);
        await _publishEndpoint.Publish(eventMessage, cancellationToken).ConfigureAwait(false);
        await _basketRepository.DeleteAsync(request.BasketCheckoutDto.UserName, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Basket published for {userName}",request.BasketCheckoutDto.UserName);
    }
}
