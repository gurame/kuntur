using System.Reflection;
using Kuntur.Worker.Host;
using Kuntur.Worker.Host.Infrastructure;
using Kuntur.Worker.Host.Diagnostics;

var builder = Host.CreateApplicationBuilder(args);

builder.AddOpenTelemetry();

// Add Event Handlers
List<Assembly> assemblies = [
    typeof(Kuntur.Worker.Notifications.WorkerMarker).Assembly,
];
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies([.. assemblies]);
});

// Add Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
