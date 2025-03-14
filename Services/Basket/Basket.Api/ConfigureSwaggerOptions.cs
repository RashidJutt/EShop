using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _versionDescriptionProvider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider versionDescriptionProvider)
    {
        _versionDescriptionProvider = versionDescriptionProvider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        foreach(var description in _versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Basket.API",
            Version = description.ApiVersion.ToString(),
            Description = $"Basket api version {description.ApiVersion}"
        };

        return info;
    }
}
