using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Configurations;

internal class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable(nameof(Subscription), "marketplace");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new SubscriptionId(value))
            .HasColumnName(nameof(SubscriptionId))
            .ValueGeneratedNever();

        builder.Property("_maxSellers")
            .HasColumnName("MaxSellers");

        builder.Property<AdminId>("_adminId")
            .HasConversion(
                id => id.Value,
                value => new AdminId(value))
            .HasColumnName(nameof(AdminId))
            .ValueGeneratedNever();

        builder.Property(s => s.SubscriptionType)
            .HasConversion(
                subscriptionType => subscriptionType.Value,
                value => SubscriptionType.FromValue(value));
    }
}