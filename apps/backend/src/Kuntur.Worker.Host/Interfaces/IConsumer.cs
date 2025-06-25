namespace Kuntur.Worker.Host.Interfaces;

public interface IConsumer
{
    Task ConsumeAsync(CancellationToken ct);
}