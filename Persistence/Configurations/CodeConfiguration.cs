using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CodeConfiguration : IEntityTypeConfiguration<Code>
{
    public void Configure(EntityTypeBuilder<Code> builder)
    {
        builder.HasKey(e => new { e.AppUserId, e.ExerciseId })
            .HasName("PK__Code__16628DE2");

        builder.Property(e => e.SourceCode).IsRequired();

        builder.HasOne(d => d.ProgrammingLanguage)
            .WithMany(p => p.Codes)
            .HasForeignKey(d => d.ProgrammingLanguageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Code__ProgrammingLanguage");

        builder.HasOne(d => d.Submission)
            .WithOne(p => p.Code)
            .HasForeignKey<Code>(d => new { d.AppUserId, d.ExerciseId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Code__Submission");
    }
}
