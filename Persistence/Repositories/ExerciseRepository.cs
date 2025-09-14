using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ExerciseRepository : RepositoryBase<Exercise>, IExerciseRepository
{
    public ExerciseRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
}