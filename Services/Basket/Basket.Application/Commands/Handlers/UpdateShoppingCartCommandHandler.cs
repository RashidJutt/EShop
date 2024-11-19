using AutoMapper;
using Basket.Application.Dtos;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Commands.Handlers;

public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartDto>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateShoppingCartCommandHandler> _logger;

    public UpdateShoppingCartCommandHandler(IBasketRepository basketRepository, IMapper mapper, ILogger<UpdateShoppingCartCommandHandler> logger)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ShoppingCartDto> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var shoppingCart = _mapper.Map<ShoppingCart>(request.ShoppingCart);
        var updatedShoppingCart = await _basketRepository.UpdateAsync(shoppingCart, cancellationToken);

        _logger.LogInformation("Successfully updated shopping cart for UserName: {UserName}", request.ShoppingCart.UserName);
        return _mapper.Map<ShoppingCartDto>(updatedShoppingCart);
    }
}
