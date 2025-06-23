using Kuntur.API.Common.Infrastructure.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Configurations;
public class OutboxIntegrationEventConfiguration : IEntityTypeConfiguration<OutboxIntegrationEvent>
{
    public void Configure(EntityTypeBuilder<OutboxIntegrationEvent> builder)
    {
        builder.ToTable(nameof(OutboxIntegrationEvent), "marketplace");

        builder.Property(o => o.Id)
            .HasColumnName("Id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(o => o.EventName);
        builder.Property(o => o.EventContent);
    }
}
