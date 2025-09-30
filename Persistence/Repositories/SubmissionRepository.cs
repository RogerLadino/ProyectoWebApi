using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SubmissionRepository : RepositoryBase<Submission>, ISubmissionRepository
{
    public SubmissionRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
    public new async Task<IEnumerable<Submission>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.AppUser)
            .ToListAsync();
    }
    public new async Task<Submission?> GetByIdAsync(int exerciseId, int id)
    {
        return await _dbSet
            .Include(s => s.AppUser)
            .FirstOrDefaultAsync(s => s.AppUserId.Equals(id) && s.ExerciseId.Equals(exerciseId));
    }
}
