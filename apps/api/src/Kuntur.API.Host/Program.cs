using Kuntur.API.Host;

using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting Kuntur API Host");

var builder = WebApplication.CreateBuilder(args);

builder.AddKunturApiServices(logger);

builder.AddKunturModules(logger);

var app = builder.Build();

app.UseKunturApi();

app.Run();