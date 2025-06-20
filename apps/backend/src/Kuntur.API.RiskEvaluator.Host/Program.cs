using Kuntur.API.RiskEvaluator.Host.Diagnostics;
using Kuntur.API.RiskEvaluator.Host.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.AddApiDiagnostics();

var app = builder.Build();

app.MapGrpcService<EvaluatorService>();
app.MapGet("/", () => "Kuntur Risk Evaluator API is running. Use gRPC to evaluate risks.");

app.Run();
