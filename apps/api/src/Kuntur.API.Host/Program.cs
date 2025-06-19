using Kuntur.API.Host;
using Microsoft.Build.Framework;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting Kuntur API Host");

var builder = WebApplication.CreateBuilder(args);
{
  builder.AddServices(logger)
         .AddModules(logger);
}

var app = builder.Build();
{
  app.UseApi();
  app.Run();
}