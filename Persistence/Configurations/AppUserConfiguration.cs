using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__AppUser__3214EC07");

        builder.Property(e => e.Email).IsRequired().HasMaxLength(45);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(45);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(45);
        builder.Property(e => e.Password).IsRequired().HasMaxLength(128);

        builder.HasOne(d => d.AppRole)
            .WithMany(p => p.AppUsers)
            .HasForeignKey(d => d.AppRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__AppUser__AppRole");
    }
}
