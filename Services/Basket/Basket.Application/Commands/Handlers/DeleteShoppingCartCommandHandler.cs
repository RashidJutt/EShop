using Basket.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Commands.Handlers;

public class DeleteShoppingCartCommandHandler : IRequestHandler<DeleteShoppingCartCommand>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<DeleteShoppingCartCommandHandler> _logger;

    public DeleteShoppingCartCommandHandler(IBasketRepository basketRepository, ILogger<DeleteShoppingCartCommandHandler> logger)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteAsync(request.Username, cancellationToken);
        _logger.LogInformation("Successfully deleted shopping cart for username: {Username}", request.Username);
    }
}
