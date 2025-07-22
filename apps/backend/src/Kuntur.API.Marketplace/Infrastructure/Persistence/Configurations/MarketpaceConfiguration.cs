using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.TenantMarketplaceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Configurations;

internal class MarketplaceConfiguration : IEntityTypeConfiguration<TenantMarketplace>
{
    public void Configure(EntityTypeBuilder<TenantMarketplace> builder)
    {
        builder.ToTable("TenantMarketplace", "marketplace");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new MarketplaceId(value))
            .HasColumnName(nameof(MarketplaceId))
            .ValueGeneratedNever();

        builder.Property(p => p.TaxId)
            .HasColumnName("TaxId");

        builder.Property("_name")
            .HasColumnName("Name");

        builder.Property("_maxSellers")
            .HasColumnName("MaxSellers");

        builder.Property<SubscriptionId>("_subscriptionId")
            .HasConversion(
                id => id.Value,
                value => new SubscriptionId(value))
            .HasColumnName(nameof(SubscriptionId))
            .ValueGeneratedNever();
    }
}