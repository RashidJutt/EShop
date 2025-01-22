using Discount.API.Services;
using Discount.Infrastructure.Extentions;
using ProtoBuf.Grpc.Server;
using Discount.Application.Extentions;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddCodeFirstGrpc();
        builder.Services.AddSingleton<IDiscountService, DiscountService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        var app = builder.Build();

        app.InitializeDatabase();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapGrpcService<DiscountService>();

        app.MapGet("/proto/discount.proto", ctx =>
        {
            ctx.Response.ContentType = "text/plain";

            var schemaGenerator = new ProtoBuf.Grpc.Reflection.SchemaGenerator();
            var schema = schemaGenerator.GetSchema(typeof(IDiscountService));

            return ctx.Response.WriteAsync(schema);
        });

        app.MapGet("/", ctx =>
        {
            ctx.Response.ContentType = "text/plain";

            return ctx.Response.WriteAsync("Communication with grpc endpoints must be made through a grpc client");
        });

        app.Run();
    }
}