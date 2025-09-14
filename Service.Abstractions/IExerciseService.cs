using Shared.DTOs.Exercise;

namespace Service.Abstractions;

public interface IExerciseService : IServiceBase<ExerciseDto>
{
    Task<ExerciseDto> CreateAsync(ExerciseCreationDto exerciseCreationDto);
}
