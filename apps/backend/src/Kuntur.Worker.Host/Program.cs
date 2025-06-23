using System.Reflection;
using Kuntur.Worker.Host;
using Kuntur.Worker.Host.Settings;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

// Add Event Handlers
List<Assembly> assemblies = [
    typeof(Kuntur.Worker.Notifications.WorkerMarker).Assembly,
];
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies([.. assemblies]);
});

// Add Options
builder.Services.Configure<MessageBrokerSettings>(
    builder.Configuration.GetSection(MessageBrokerSettings.Section));

// Add Background Services
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
