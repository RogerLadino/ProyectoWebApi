using Domain.Entities;
using Domain.Exceptions;
using Domain.Realtime;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.DTOs.Code;
using Shared.DTOs.Submission;

public class CodeService : ICodeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IRealtimeManager _realtimeManager;
    private readonly IServiceManager _serviceManager;

    public CodeService(IRepositoryManager repositoryManager, IRealtimeManager realtimeManager, IServiceManager serviceManager)
    {
        _repositoryManager = repositoryManager;
        _realtimeManager = realtimeManager;
        _serviceManager = serviceManager;
    }

    public async Task<CodeUpdateDto?> GetByIdAsync(int exerciseId, int userId)
    {
        var code = (await _repositoryManager.CodeRepository.FindByConditionAsync(
            c => c.AppUserId.Equals(dto.AppUserId) && c.ExerciseId.Equals(dto.ExerciseId)))
            .FirstOrDefault();

        if (code is not null)
        {
            return code.Adapt<CodeUpdateDto>();
        }


        await _serviceManager.SubmissionService.CreateAsync(new SubmissionCreationDto
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
