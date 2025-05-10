using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Kuntur.API.Marketplace.Domain.SubscriptionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Marketplace.Infrastructure.Persistence.Configurations;

internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable(nameof(Admin), "marketplace");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new AdminId(value))
            .HasColumnName(nameof(AdminId))
            .ValueGeneratedNever();

        builder.Property(u => u.UserId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .HasColumnName(nameof(UserId))
            .ValueGeneratedNever();

        builder.Property(u => u.SubscriptionId)
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? new SubscriptionId(value.Value) : null)
            .HasColumnName(nameof(SubscriptionId))
            .ValueGeneratedNever();
    }
}