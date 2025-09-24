using Domain.Entities;
using Domain.Exceptions.Exceptions;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.DTOs.Submission;

namespace Services;
public class SubmissionService : ISubmissionService
{
    private readonly IRepositoryManager _repositoryManager;

    public SubmissionService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<SubmissionDto>> GetAllAsync(int exerciseId)
    {
        var submissions = await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId);

        return submissions.Adapt<IEnumerable<SubmissionDto>>();
    }

    public async Task<SubmissionDto> GetByIdAsync(int userId, int exerciseId)
    {
        var submission = (await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId && s.AppUserId == userId))
            .FirstOrDefault();

        if (submission == null)
            return new SubmissionDto
            {
                AppUserId = userId,
                ExerciseId = exerciseId,
                Grade = 0,
                Status = 0,
                SubmittedAt = null
            };

        return submission.Adapt<SubmissionDto>();
    }

    public async Task AssignGrade(int userId, int exerciseId, int grade)
    {
        var submission = (await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId && s.AppUserId == userId))
            .FirstOrDefault();

        if (submission == null)
        {
            await CreateAsync(new SubmissionCreationDto
            {
                AppUserId = userId,
                ExerciseId = exerciseId,
                Grade = grade,
                Status = 0,
                SubmittedAt = null
            });

            return;
        }

        submission.Grade = grade;

        _repositoryManager.SubmissionRepository.Update(submission);
        await _repositoryManager.SaveChangesAsync();
    }

    public async Task<SubmissionDto> CreateAsync(SubmissionCreationDto submissionForCreationDto)
    {
        var submission = submissionForCreationDto.Adapt<Submission>();

        var submissionExists = await _repositoryManager.SubmissionRepository
            .AnyAsync(s => s.ExerciseId == submission.ExerciseId
                        && s.AppUserId == submission.AppUserId);

        if (submissionExists)
            throw new SubmissionAlreadyExistsException("Submission already exists");

        _repositoryManager.SubmissionRepository.Add(submission);
        await _repositoryManager.SaveChangesAsync();

        var code = new Code
        {
            AppUserId = submission.AppUserId,
            ExerciseId = submission.ExerciseId,
            SourceCode = string.Empty,
            Attempts = 0,
            ProgrammingLanguageId = 1
        };

        _repositoryManager.CodeRepository.Add(code);
        await _repositoryManager.SaveChangesAsync();

        return submission.Adapt<SubmissionDto>();
    }
}
