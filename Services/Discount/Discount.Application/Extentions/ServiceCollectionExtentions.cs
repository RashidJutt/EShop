using Discount.Application.Commands;
using Discount.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Discount.Application.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
        }, Assembly.GetAssembly(typeof(DiscountMapProfile)));

        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateCouponCommand).Assembly));
    }
}
