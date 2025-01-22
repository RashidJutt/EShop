using Discount.Core.Repositories;
using Discount.Infrastructure.Helpers;
using Discount.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Infrastructure.Extentions;

public static class ServiceCollectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<DataContext>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
    }
}
