using Domain.Repositories;

namespace Domain.Repositories;

public interface IRepositoryManager
{
    IExerciseRepository ExerciseRepository { get; }
    Task SaveChangesAsync();
}