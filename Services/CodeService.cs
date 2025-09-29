using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Exceptions;
using Domain.Realtime;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.DTOs.Code;
using Shared.DTOs.Submission;

namespace Services;

public class CodeService : ICodeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IRealtimeManager _realtimeManager;
    private readonly ISubmissionService _submissionService;

    public CodeService(IRepositoryManager repositoryManager, IRealtimeManager realtimeManager, ISubmissionService submissionService)
    {
        _repositoryManager = repositoryManager;
        _realtimeManager = realtimeManager;
        _submissionService = submissionService;
    }

    public async Task<CodeUpdateDto?> GetByIdAsync(int exerciseId, int userId)
    {
        var code = (await _repositoryManager.CodeRepository.FindByConditionAsync(
            c => c.AppUserId.Equals(userId) && c.ExerciseId.Equals(exerciseId)))
            .FirstOrDefault();

        if (code is not null)
        {
            return code.Adapt<CodeUpdateDto>();
        }

        var submissionForCreationDto = new SubmissionCreationDto
        {
            AppUserId = userId,
            ExerciseId = exerciseId,
            Grade = 0,
            Status = 0,
            SubmittedAt = null
        };

        var submission = submissionForCreationDto.Adapt<Submission>();

        var submissionExists = await _repositoryManager.SubmissionRepository
            .AnyAsync(s => s.ExerciseId == submission.ExerciseId
                        && s.AppUserId == submission.AppUserId);

        if (submissionExists)
            throw new SubmissionAlreadyExistsException("Submission already exists");

        _repositoryManager.SubmissionRepository.Add(submission);
        await _repositoryManager.SaveChangesAsync();

        var codeForCreation = new Code
        {
            AppUserId = submission.AppUserId,
            ExerciseId = submission.ExerciseId,
            SourceCode = string.Empty,
            Attempts = 0,
            ProgrammingLanguageId = 1
        };

        _repositoryManager.CodeRepository.Add(codeForCreation);
        await _repositoryManager.SaveChangesAsync();

        return null;
    }

    public async Task UpdateCodeAsync(CodeUpdateDto dto)
    {
        var code = (await _repositoryManager.CodeRepository.FindByConditionAsync(
            c => c.AppUserId.Equals(dto.AppUserId) && c.ExerciseId.Equals(dto.ExerciseId)))
            .FirstOrDefault();

        if (code is null)
            throw new CodeNotFoundException("Code not found");

        dto.Adapt<Code>();

        _repositoryManager.CodeRepository.Update(code);

        await _repositoryManager.SaveChangesAsync();

        await _realtimeManager.CodeRealtime.NotifyCodeChangedAsync(dto);
    }
}
