using Basket.Core.Entities;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetAsync(string userName, CancellationToken cancellationToken);
    Task<ShoppingCart> UpdateAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken);
    Task DeleteAsync(string userName, CancellationToken cancellationToken);
}
