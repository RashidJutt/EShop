using Basket.Application.Clients;
using Basket.Core.Repositories;
using Basket.Infrastructure.Caching;
using Basket.Infrastructure.Clients;
using Basket.Infrastructure.Encryption;
using Basket.Infrastructure.Repositories;
using Discount.API.Services;
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
        services.AddScoped<IDiscountServiceClient, DiscountServiceClient>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("CacheSettings:ConnectionString").Value;
        });

        services.AddGrpcClient<DiscountService.DiscountServiceClient>(
            options =>
            {
                options.Address = new Uri(configuration.GetSection("GrpcSettings:DiscountUrl").Value ?? "http://localhost:8002");
            });
    }
}
