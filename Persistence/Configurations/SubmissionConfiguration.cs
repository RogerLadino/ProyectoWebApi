using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasKey(e => new { e.AppUserId, e.ExerciseId })
            .HasName("PK__Submission__16628DE2");

        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.SubmittedAt).IsRequired();

        builder.HasOne(d => d.AppUser)
            .WithMany(p => p.Submissions)
            .HasForeignKey(d => d.AppUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Submission__AppUser");

        builder.HasOne(d => d.Exercise)
            .WithMany(p => p.Submissions)
            .HasForeignKey(d => d.ExerciseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Submission__Exercise");
    }
}
