using Basket.Core.Entities;
using Basket.Core.Repositories;
using Basket.Infrastructure.Caching;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IEncryptedCache _cache;

    public BasketRepository(IEncryptedCache cache)
    {
        _cache = cache;
    }


    public async Task<ShoppingCart?> GetAsync(string userName, CancellationToken cancellationToken)
    {
        return await _cache.GetAsync<ShoppingCart>(userName, cancellationToken);
    }

    public async Task<ShoppingCart> UpdateAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        await _cache.SetAsync(shoppingCart.UserName, shoppingCart, cancellationToken);
        return (await GetAsync(shoppingCart.UserName, cancellationToken))!;
    }

    public async Task DeleteAsync(string userName, CancellationToken cancellationToken)
    {
        await _cache.DeleteAsync(userName, cancellationToken);
    }
}
