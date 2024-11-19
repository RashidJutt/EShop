namespace Basket.Infrastructure.Caching;

public interface IEncryptedCache
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken);
    Task DeleteAsync(string key, CancellationToken cancellationToken);
}
