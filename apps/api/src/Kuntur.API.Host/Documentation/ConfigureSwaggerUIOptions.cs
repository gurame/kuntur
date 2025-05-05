using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Kuntur.API.Host.Documentation;

public class ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider p) : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = p;

    public void Configure(SwaggerUIOptions c)
    {
        c.DocumentTitle = "Kuntur API";
        c.RoutePrefix   = "docs";

        // TODO: Uncomment this when we have multiple versions fixed
        // foreach (var desc in _provider.ApiVersionDescriptions)
        // {
        //     c.SwaggerEndpoint(
        //       $"/docs/{desc.GroupName}/swagger.json",
        //       desc.GroupName.ToUpperInvariant());
        // }

        c.SwaggerEndpoint("/docs/v1.0/swagger.json", "V1.0");
        c.SwaggerEndpoint("/docs/v2.0/swagger.json", "V2.0");
    }
}