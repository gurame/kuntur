namespace Kuntur.API.Common.Infrastructure.IntegrationEvents;
public record OutboxIntegrationEvent
{
    public int Id { get; init; } = default;
    public string EventName { get; init; } = default!;
    public string EventContent { get; init; } = default!;
    public OutboxIntegrationEvent()
    {
    }
    public OutboxIntegrationEvent(string eventName, string eventContent)
    {
        EventName = eventName;
        EventContent = eventContent;
    }
}