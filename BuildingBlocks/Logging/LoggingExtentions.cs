using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Logging;

public static class LoggingExtentions
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureSirilog => (context, configuration) =>
    {
        var enviroment = context.HostingEnvironment;
        configuration
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", enviroment.ApplicationName)
            .Enrich.WithProperty("Environment", enviroment.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .WriteTo.Console();

        if (enviroment.IsDevelopment())
        {
            configuration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
            configuration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
            configuration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
            configuration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
        }

        var elasticConfiguration = context.Configuration.GetSection("ElasticConfiguration:Uri").Value;
        if (!string.IsNullOrEmpty(elasticConfiguration))
        {
            configuration.WriteTo.Elasticsearch(new[] { new Uri(elasticConfiguration) }, opts =>
              {
                  opts.DataStream = new DataStreamName("EShop");
                  opts.BootstrapMethod = BootstrapMethod.Failure;
                  opts.ConfigureChannel = channelOpts =>
                  {
                      channelOpts.BufferOptions = new BufferOptions
                      {
                          ExportMaxConcurrency = 10,
                      };
                  };
              });
        }
    };
}
