
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate;
using Kuntur.API.Marketplace.Domain.MarketplaceAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Configurations;
internal class MaerketplaceConfiguration : IEntityTypeConfiguration<MarketplaceAgg>
{
    public void Configure(EntityTypeBuilder<MarketplaceAgg> builder)
    {
        builder.ToTable("Marketplace", "marketplace");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new MarketplaceId(value))
            .HasColumnName(nameof(MarketplaceId))
            .ValueGeneratedNever();

        builder.Property("_taxId")
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