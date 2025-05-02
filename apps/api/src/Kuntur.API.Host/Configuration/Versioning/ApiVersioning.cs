using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace Kuntur.API.Host.Configuration.Versioning;

public static class ApiVersioning
{
    public static ApiVersionSet VersionSet { get; private set; } = null!;

    public static WebApplicationBuilder AddApiVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1.0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddApiExplorer(x =>
        {
            x.GroupNameFormat = "'v'VV";
            x.SubstituteApiVersionInUrl = false;
        });

        return builder;
    }

    public static WebApplication UseApiVersioning(this WebApplication app)
    {
        VersionSet = app.NewApiVersionSet()
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .ReportApiVersions()
            .Build();

        return app;
    }
}
