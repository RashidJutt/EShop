using Asp.Versioning;
using EventBus;
using EventBus.Events;
using Logging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Consumers;
using Ordering.Application.Commands;
using Ordering.Application.Profiles;
using Ordering.Application.Services;
using Ordering.Core.Contracts;
using Ordering.Core.Repositories;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Repositories;
using Serilog;
using System.Reflection;

namespace Ordering.API;

public static class HostExtentions
{
    public static async Task InitializeAsync(this WebApplication app)
    {
        await app.MigrateDatabase();
        await app.SeedDatabase();
    }
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        string migrationAssembly = Assembly.GetAssembly(typeof(OrderDbContext))!.GetName().Name!;

        var connectionString = configuration.GetConnectionString("OrderingConnectionString")!;

        builder.Services.AddDbContext<OrderDbContext>(ctx =>
        {
            ctx.UseSqlServer(connectionString);
        });

        return builder;
    }

    public static WebApplicationBuilder AddRepository(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        return builder;
    }

    public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));
        return builder;
    }

    public static WebApplicationBuilder AddAutomapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(cfg =>
          {
              cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
          }, Assembly.GetAssembly(typeof(OrderMapProfile)));

        return builder;
    }

    public static WebApplicationBuilder AddVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        return builder;
    }

    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(configure =>
        {
            configure.AddConsumer<BasketCheckoutConsumer>();
            configure.AddConsumer<BasketCheckoutV2Consumer>();
            configure.UsingRabbitMq((cxt, config) =>
            {
                var eventBusConnectionString = builder.Configuration["EventBusSettings:HostAddress"];
                config.Host(eventBusConnectionString);
                config.ReceiveEndpoint(EventConstants.BasketCheckoutQueue, cfg =>
                {
                    cfg.ConfigureConsumer<BasketCheckoutConsumer>(cxt);
                });

                config.ReceiveEndpoint(EventConstants.BasketCheckoutV2Queue, cfg =>
                {
                    cfg.ConfigureConsumer<BasketCheckoutV2Consumer>(cxt);
                });
            });
        });
        return builder;
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(LoggingExtentions.ConfigureSirilog);
        return builder;
    }
}
