using Kuntur.API.Host;
using Serilog;

// TODO: Use the ILoggerFactory to create a logger instead of using Serilog directly
var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

//logger.Information("Starting Kuntur API Host");

var builder = WebApplication.CreateBuilder(args);
{
  // get ILoggerFactory from the builder
  
  builder.AddServices(logger)
         .AddModules(logger);
}

var app = builder.Build();
{
  app.UseApi();
  app.Run();
}