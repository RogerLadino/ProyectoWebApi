using Shared.DTOs.Exercise;
using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using Domain.Exceptions;
using Mapster;

namespace Services;

public class ExerciseService : IExerciseService
{
    private readonly IRepositoryManager _repositoryManager;

    public ExerciseService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<ExerciseDto>> GetAllAsync()
    {
        var exercises = await _repositoryManager.ExerciseRepository.GetAllAsync();
        return exercises.Adapt<IEnumerable<ExerciseDto>>();
    }

    public async Task<ExerciseDto> GetByIdAsync(int exerciseId)
    {
        var exercise = await _repositoryManager.ExerciseRepository.GetByIdAsync(exerciseId);

        if (exercise is null)
            throw new ExerciseNotFoundException("No exercise exists with the given ID");

        return exercise.Adapt<ExerciseDto>();
    }

    public async Task<ExerciseDto> CreateAsync(int classroomId, ExerciseCreationDto exerciseForCreationDto)
    {
        exerciseForCreationDto.ClassroomId = classroomId;
        var exercise = exerciseForCreationDto.Adapt<Exercise>();

        var exerciseExists = await _repositoryManager.ExerciseRepository
            .AnyAsync(e => e.Name.Equals(exercise.Name) && e.ClassroomId.Equals(exercise.ClassroomId));

        if (exerciseExists)
            throw new ExerciseAlreadyExistsException("An exercise with the same name already exists");

        _repositoryManager.ExerciseRepository.Add(exercise);
        await _repositoryManager.SaveChangesAsync();

        return exercise.Adapt<ExerciseDto>();
    }

    public async Task UpdateAsync(int exerciseId, ExerciseDto exerciseForUpdateDto)
    {
        var exercise = await _repositoryManager.ExerciseRepository.GetByIdAsync(exerciseId);

        if (exercise is null)
            throw new ExerciseNotFoundException("No exercise exists with the given ID");

        exerciseForUpdateDto.Adapt(exercise);

        var exerciseExists = await _repositoryManager.ExerciseRepository
            .AnyAsync(e => e.Name.Equals(exercise.Name) && e.Id != exerciseId);

        if (exerciseExists)
            throw new ExerciseAlreadyExistsException("An exercise with the same name already exists");

        await _repositoryManager.SaveChangesAsync();
    }

    public async Task DeleteAsync(int exerciseId)
    {
        var exercise = await _repositoryManager.ExerciseRepository.GetByIdAsync(exerciseId);

        if (exercise is null)
            throw new ExerciseNotFoundException("No exercise exists with the given ID");

        _repositoryManager.ExerciseRepository.Remove(exercise);
        await _repositoryManager.SaveChangesAsync();
    }
}
