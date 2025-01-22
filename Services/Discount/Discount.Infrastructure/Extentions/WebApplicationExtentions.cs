using Discount.Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Infrastructure.Extentions;

public static class WebApplicationExtentions
{
    public static async void InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.InitAsync();
    }
}
