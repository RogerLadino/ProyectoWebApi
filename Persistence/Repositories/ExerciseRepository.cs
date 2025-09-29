using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Persistence.Repositories;

public class ExerciseRepository : RepositoryBase<Exercise>, IExerciseRepository
{
    public ExerciseRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
    public new async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.TestCases)
            .ToListAsync();
    }

    public new async Task<Exercise?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.TestCases)
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
    }
}