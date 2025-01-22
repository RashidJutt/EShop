
using Basket.Infrastructure.Encryption;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Basket.Infrastructure.Caching;

public class EncryptedCache : IEncryptedCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly IEncryptionService _encryptionService;

    public EncryptedCache(
        IDistributedCache distributedCache,
        IEncryptionService encryptionService)
    {
        _distributedCache = distributedCache;
        _encryptionService = encryptionService;
    }
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        var encryptedValue = await _distributedCache.GetStringAsync(GetKey(key), cancellationToken);
        if (string.IsNullOrEmpty(encryptedValue))
        {
            return null;
        }

        var decryptedValue = _encryptionService.Decrypt(encryptedValue);
        return JsonSerializer.Deserialize<T>(decryptedValue);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        var encryptedValue = _encryptionService.Encrypt(serializedValue);
        await _distributedCache.SetStringAsync(GetKey(key), encryptedValue, cancellationToken);
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(GetKey(key), cancellationToken);
    }

    private string GetKey(string key)
    {
        return $"Basket:{key}";
    }
}
