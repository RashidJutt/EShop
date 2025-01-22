using AutoMapper;
using Basket.Application.Clients;
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
    private readonly IDiscountServiceClient _discountServiceClient;

    public UpdateShoppingCartCommandHandler(
        IBasketRepository basketRepository,
        IMapper mapper,
        ILogger<UpdateShoppingCartCommandHandler> logger,
        IDiscountServiceClient discountServiceClient)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
        _logger = logger;
        _discountServiceClient = discountServiceClient;
    }

    public async Task<ShoppingCartDto> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        await ApplyDiscountAsync(request.ShoppingCart, cancellationToken);

        var shoppingCartEntity = _mapper.Map<ShoppingCart>(request.ShoppingCart);
        var updatedShoppingCart = await _basketRepository.UpdateAsync(shoppingCartEntity, cancellationToken);

        _logger.LogInformation("Shopping cart updated for UserName: {UserName}", request.ShoppingCart.UserName);

        return _mapper.Map<ShoppingCartDto>(updatedShoppingCart);
    }

    private async Task ApplyDiscountAsync(ShoppingCartDto shoppingCart, CancellationToken cancellationToken)
    {
        foreach (var item in shoppingCart.Items)
        {
            var discount = await _discountServiceClient.GetCoupon(item.ProductName);

            if (discount?.Amount > 0)
            {
                item.Price -= discount.Amount;
                _logger.LogInformation("Applied discount of {Amount} to item {ProductName} for UserName: {UserName}",
                    discount.Amount, item.ProductName, shoppingCart.UserName);
            }
        }
    }
}
