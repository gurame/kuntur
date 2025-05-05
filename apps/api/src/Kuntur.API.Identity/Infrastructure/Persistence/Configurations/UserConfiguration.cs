using Kuntur.API.Identity.Domain.UserAggregate;
using Kuntur.API.Identity.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuntur.API.Identity.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .HasColumnName(nameof(UserId))
            .ValueGeneratedNever();

        builder.ComplexProperty<Name>("_name", cp =>
        {
            cp.Property(n => n.FirstName)
              .HasColumnName(nameof(Name.FirstName))
              .IsRequired()
              .HasMaxLength(50);

            cp.Property(n => n.LastName)
              .HasColumnName(nameof(Name.LastName))
              .IsRequired()
              .HasMaxLength(50);
        });

        builder.ComplexProperty(u => u.EmailAddress, cp =>
        {
            cp.Property(e => e.Value)
                .HasColumnName(nameof(EmailAddress))
                .IsRequired()
                .HasMaxLength(200);
        });

        builder.ComplexProperty<PhoneNumber>("_phoneNumber", cp =>
        {
            cp.Property(p => p.Value)
                .HasColumnName(nameof(PhoneNumber))
                .IsRequired()
                .HasMaxLength(20);
        });

        builder.Property<AdminId?>("_adminId")
            .HasConversion(
                id => id.HasValue ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? new AdminId(value.Value) : null)
            .HasColumnName(nameof(AdminId))
            .ValueGeneratedNever();
    }
}