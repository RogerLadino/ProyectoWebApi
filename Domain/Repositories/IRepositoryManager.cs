using Domain.Repositories;
using Persistence.Repositories;

namespace Domain.Repositories;

public interface IRepositoryManager
{
    IExerciseRepository ExerciseRepository { get; }
    Task SaveChangesAsync();
    IUsuarioRepository UsuarioRepository { get; }

    IClassroomRepository ClassroomRepository { get; }


}