using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
{
    public void Configure(EntityTypeBuilder<TestCase> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__TestCase__3214EC07");

        builder.Property(e => e.FunctionName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.InputData).IsRequired();
        builder.Property(e => e.ExpectedOutput).IsRequired();

        builder.HasOne(d => d.Exercise)
            .WithMany(p => p.TestCases)
            .HasForeignKey(d => d.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK__TestCase__Exercise");
    }
}
