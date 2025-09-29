using Domain.Entities;
using Domain.Exceptions;
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


        await _submissionService.CreateAsync(new SubmissionCreationDto
        {
            AppUserId = userId,
            ExerciseId = exerciseId,
            Grade = 0,
            Status = 0,
            SubmittedAt = null
        });

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
