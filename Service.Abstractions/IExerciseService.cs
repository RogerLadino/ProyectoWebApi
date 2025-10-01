using Shared.DTOs.Exercise;

namespace Service.Abstractions;

public interface IExerciseService
{
    Task<ExerciseDto> CreateAsync(int classroomId, ExerciseCreationDto exerciseCreationDto);
    Task<IEnumerable<ExerciseDto>> GetAllAsync(int classroomId);
    Task<ExerciseDto> GetByIdAsync(int exerciseId);
    Task UpdateAsync(int id, ExerciseDto exerciseUpdateDto);
    Task DeleteAsync(int id);
}
