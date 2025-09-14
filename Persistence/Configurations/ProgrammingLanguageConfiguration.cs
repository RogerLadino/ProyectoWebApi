using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ProgrammingLanguageConfiguration : IEntityTypeConfiguration<ProgrammingLanguage>
{
    public void Configure(EntityTypeBuilder<ProgrammingLanguage> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__ProgrammingLanguage__3214EC07");

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(45);
    }
}
