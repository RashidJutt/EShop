using Basket.Application.Exceptions;
using Basket.Core.Entities;
using MediatR;

namespace Basket.Application.Validators;

public class ShoppingCartValidator : IShoppingCartValidator
{
    public void Validate(ShoppingCart? cart, string userName)
    {
        if (cart == null)
        {
            throw new NotFoundException($"Basket with username {userName} not found.");
        }
    }
}
