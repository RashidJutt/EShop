using Asp.Versioning;
using AutoMapper.Internal;
using Catalog.API.Middlewares;
using Catalog.Application.Commands;
using Catalog.Application.Profiles;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Logging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(LoggingExtentions.ConfigureSirilog);
builder.Services.AddControllers();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Catalog.API", Version = "v1" }); });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
}, Assembly.GetAssembly(typeof(CatalogMappingProfile)));

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    await SeedData(app);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.Run();

static async Task SeedData(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var catalogContext = scope.ServiceProvider.GetRequiredService<ICatalogContext>();
        await catalogContext.SeedDataAsync();
    }
}