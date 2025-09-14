using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Classroom__3214EC07");

        builder.Property(e => e.Name).IsRequired().HasMaxLength(45);
        builder.Property(e => e.Code).IsRequired().HasMaxLength(20);

        builder.HasMany(d => d.AppUsers)
            .WithMany(p => p.Classrooms)
            .UsingEntity<Dictionary<string, object>>(
                "UserClassroom",
                r => r.HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey("AppUserId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserClassroom__AppUser"),
                l => l.HasOne<Classroom>()
                    .WithMany()
                    .HasForeignKey("ClassroomId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserClassroom__Classroom"),
                j =>
                {
                    j.HasKey("ClassroomId", "AppUserId")
                        .HasName("PK__UserClassroom");
                    j.ToTable("UserClassroom");
                });
    }
}
