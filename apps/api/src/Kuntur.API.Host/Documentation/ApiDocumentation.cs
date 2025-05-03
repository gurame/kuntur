using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Kuntur.API.Host.Documentation;

public static class ApiDocumentation
{
    public static WebApplicationBuilder AddApiDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();

        return builder;
    }

    public static IApplicationBuilder UseApiDocumentation(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "docs/{documentName}/swagger.json";
        });
        app.UseSwaggerUI();

        return app;
    }
}