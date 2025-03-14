using Basket.Application.Profiles;
using Basket.Application.Validators;
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

        var validatorType = typeof(IShoppingCartValidator);
        var validators = Assembly.GetAssembly(validatorType)!
                                 .GetTypes()
                                 .Where(t => validatorType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var validator in validators)
        {
            services.AddScoped(validatorType, validator);
        }
    }
}