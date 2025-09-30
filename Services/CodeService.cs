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

    public CodeService(IRepositoryManager repositoryManager, IRealtimeManager realtimeManager)
    {
        _repositoryManager = repositoryManager;
        _realtimeManager = realtimeManager;
    }

    public async Task<CodeUpdateDto> GetByIdAsync(int exerciseId, int userId)
    {
        var code = (await _repositoryManager.CodeRepository.FindByConditionAsync(
            c => c.AppUserId.Equals(userId) && c.ExerciseId.Equals(exerciseId)))
            .FirstOrDefault();

        if (code is not null)
        {
            return code.Adapt<CodeUpdateDto>();
        }

        var submission = (await _repositoryManager.SubmissionRepository.FindByConditionAsync(
            s => s.AppUserId.Equals(userId) && s.ExerciseId.Equals(exerciseId)
        ))
        .FirstOrDefault();

        var codeForCreation = new Code
        {
            AppUserId = userId,
            ExerciseId = exerciseId,
            SourceCode = string.Empty,
            Attempts = 0,
            ProgrammingLanguageId = 1,
            Submission = submission
        };

        _repositoryManager.CodeRepository.Add(codeForCreation);
        await _repositoryManager.SaveChangesAsync();

        return codeForCreation.Adapt<CodeUpdateDto>();
    }

    public async Task UpdateCodeAsync(CodeUpdateDto dto)
    {
        var code = (await _repositoryManager.CodeRepository.FindByConditionAsync(
            c => c.AppUserId.Equals(dto.AppUserId) && c.ExerciseId.Equals(dto.ExerciseId)))
            .FirstOrDefault();

        if (code is null)
            throw new CodeNotFoundException("Code not found");

        dto.Adapt(code);

        await _repositoryManager.SaveChangesAsync();

        await _realtimeManager.CodeRealtime.NotifyCodeChangedAsync(dto);
    }
}
