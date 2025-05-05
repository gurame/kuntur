using Kuntur.API.Host;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
{
  logger.Information("Starting Kuntur API Host");

  builder.AddServices(logger)
         .AddModules(logger);
}

var app = builder.Build();
{
  app.UseApi();

  app.Run();
}