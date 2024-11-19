using Basket.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basket.Application.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
        }, Assembly.GetAssembly(typeof(BasketMappingProfile)));
    }
}