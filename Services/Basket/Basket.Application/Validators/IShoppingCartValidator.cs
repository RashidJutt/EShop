using Basket.Core.Entities;

namespace Basket.Application.Validators;

public interface IShoppingCartValidator
{
    void Validate(ShoppingCart? cart, string userName);
}
