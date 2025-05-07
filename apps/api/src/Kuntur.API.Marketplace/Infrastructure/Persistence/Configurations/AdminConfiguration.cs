using Kuntur.API.Marketplace.Domain.AdminAggregate;
using Kuntur.API.Marketplace.Domain.AdminAggregate.ValueObjects;
using Kuntur.API.Marketplace.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Identity.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
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
    }
}