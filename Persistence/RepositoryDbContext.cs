using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence;

public partial class RepositoryDbContext : DbContext
{
    public RepositoryDbContext()
    {
    }

    public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);

        modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Description = "profesor" },
                new AppRole { Id = 2, Description = "alumno" }
            );

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
