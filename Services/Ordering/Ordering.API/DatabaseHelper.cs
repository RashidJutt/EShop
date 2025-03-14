using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure;
using Polly;
using Polly.Retry;

namespace Ordering.API
{
    internal static class DatabaseHelper
    {
        private static AsyncRetryPolicy retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(sleepDurationProvider: (retryCount) => TimeSpan.FromSeconds(Math.Pow(2, Math.Min(8, retryCount))));
        public static async Task MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<OrderDbContext>();
            var logger = services.GetRequiredService<ILogger<OrderDbContext>>();

            await retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    logger.LogInformation("Trying to migrate Database ({DbContext})", typeof(OrderDbContext).Name);
                    await dbContext.Database.MigrateAsync();
                    logger.LogInformation("Database migration ({DbContext}) succeeds", typeof(OrderDbContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Database migration ({DbContext}) failed", typeof(OrderDbContext).Name);
                    throw;
                }
            });
        }

        public static async Task SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<OrderDbContext>();
            var logger = services.GetRequiredService<ILogger<OrderDbContext>>();

            await retryPolicy.ExecuteAsync(async () => {
                try
                {
                    logger.LogInformation("Trying to seed Database ({DbContext})", typeof(OrderDbContext).Name);
                    await DatabaseSeeding.SeedAsyc(context,logger);
                    logger.LogInformation("Database ({DbContext}) seeding succeeds", typeof(OrderDbContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Database ({DbContext}) seeding failed", typeof(OrderDbContext).Name);
                    throw;
                }
            });
        }

    }
}
