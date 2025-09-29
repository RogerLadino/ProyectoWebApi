using Domain.Repositories;
using Persistence.Repositories;

namespace Domain.Repositories;

public interface IRepositoryManager
{
    IExerciseRepository ExerciseRepository { get; }
    ISubmissionRepository SubmissionRepository { get; }
    ICodeRepository CodeRepository { get; }
    Task SaveChangesAsync();
    IUsuarioRepository UsuarioRepository { get; }

    IClassroomRepository ClassroomRepository { get; }
    IUserClassroomRepository UserClassroomRepository { get; }
    object UserRepository { get; set; }
}