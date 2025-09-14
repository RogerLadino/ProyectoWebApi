using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Exercise__3214EC07");

        builder.Property(e => e.Name).IsRequired().HasMaxLength(45);
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

        builder.HasOne(d => d.Classroom)
            .WithMany(p => p.Exercises)
            .HasForeignKey(d => d.ClassroomId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Exercise__Classroom");
    }
}
