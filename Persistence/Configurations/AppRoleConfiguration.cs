using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__AppRole__3214EC07");

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(45);
    }
}
