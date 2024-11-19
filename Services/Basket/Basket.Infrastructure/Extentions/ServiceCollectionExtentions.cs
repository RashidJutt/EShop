using Basket.Core.Repositories;
using Basket.Infrastructure.Caching;
using Basket.Infrastructure.Encryption;
using Basket.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Infrastructure.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IEncryptedCache, EncryptedCache>();
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("CacheSettings:ConnectionString").Value;
        });
    }
}
