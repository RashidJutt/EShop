using Basket.Application.Exceptions;
using Basket.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Validators;

public class ShoppingCartValidator : IShoppingCartValidator
{
    private readonly ILogger<ShoppingCartValidator> _logger;

    public ShoppingCartValidator(ILogger<ShoppingCartValidator> logger)
    {
        _logger = logger;
    }
    public void Validate(ShoppingCart? cart, string userName)
    {
        if (cart == null)
        {
            throw new NotFoundException($"Basket with username {userName} not found.");
        }

        _logger.LogInformation("Basket of user {userName} found", userName);
    }
}
