namespace Kuntur.SharedKernel.IntegrationEvents;

public record OutboxIntegrationEvent
{
    public OutboxIntegrationEvent()
    {
    }

    public OutboxIntegrationEvent(string eventName, string eventContent)
    {
        EventName = eventName;
        EventContent = eventContent;
    }

    public int Id { get; init; } = default;
    public string EventName { get; init; } = default!;
    public string EventContent { get; init; } = default!;
}