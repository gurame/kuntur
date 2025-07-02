using System.Data;
using Dapper;
using Kuntur.API.Shared.Infrastructure.Persistence;
using Kuntur.SharedKernel.IntegrationEvents;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Outbox;

internal class OutboxRepository(IDbConnection connection, KunturDbContext dbContext) : IOutboxRepository
{
    private readonly IDbConnection _connection = connection;
    private readonly KunturDbContext _dbContext = dbContext;
    public async Task AddAsync(OutboxIntegrationEvent outboxIntegrationEvent, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO marketplace.""OutboxIntegrationEvent"" (""EventName"", ""EventContent"")
            VALUES (@EventName, @EventContent)
        ";
        await _connection.ExecuteAsync(sql, new
        {
            outboxIntegrationEvent.EventName,
            outboxIntegrationEvent.EventContent
        }, transaction: _dbContext.Database.CurrentTransaction?.GetDbTransaction());
    }

    public async Task<List<OutboxIntegrationEvent>> GetPendingEventsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT ""Id"", ""EventName"", ""EventContent""
            FROM marketplace.""OutboxIntegrationEvent"";
        ";

        var result = await _connection.QueryAsync<OutboxIntegrationEvent>(sql);
        return [.. result];
    }

    public async Task RemoveRangeAsync(IEnumerable<OutboxIntegrationEvent> outboxIntegrationEvents, CancellationToken cancellationToken = default)
    {
        const string sql = @"DELETE FROM marketplace.""OutboxIntegrationEvent"" WHERE ""Id"" = ANY(@Ids);";

        var ids = outboxIntegrationEvents.Select(e => e.Id).ToArray();

        await _connection.ExecuteAsync(sql, new { Ids = ids },
            transaction: _dbContext.Database.CurrentTransaction?.GetDbTransaction());
    }
}