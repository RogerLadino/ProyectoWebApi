using Domain.Repositories;

namespace Domain.Repositories;

public interface IRepositoryManager
{
    IExerciseRepository ExerciseRepository { get; }
    ISubmissionRepository SubmissionRepository { get; }
    ICodeRepository CodeRepository { get; }
    Task SaveChangesAsync();
}