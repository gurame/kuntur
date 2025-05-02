using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Kuntur.API.Host.Configuration.Documentation;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider p) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = p;
    public void Configure(SwaggerGenOptions options)
    {
        Serilog.Log.Logger.Information("Executing Swagger configuration");

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            Serilog.Log.Logger.Information("Adding Swagger endpoint for version {Version}", description.GroupName);
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"Kuntur API {description.GroupName}",
                    Description = "Kuntur API",
                    TermsOfService = new Uri("https://kuntur.com/terms"),
                    Version = description.ApiVersion.ToString(),
                    Contact = new OpenApiContact
                    {
                        Name = "Kuntur",
                        Url = new Uri("https://kuntur.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Kuntur License",
                        Url = new Uri("https://kuntur.com/license"),
                    }
                });
        }
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please provide a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
