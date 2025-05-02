using Serilog;
using Kuntur.API.Host.Configuration;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting Kuntur API Host");

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices(logger);

builder.AddKunturModules(logger);

var app = builder.Build();

app.UseApi();

app.Run();