using Shared.DTOs.Code;

namespace Service.Abstractions;

public interface ICodeService
{
    Task<CodeUpdateDto> GetByIdAsync(int exerciseId, int userId);
    Task UpdateCodeAsync(CodeUpdateDto dto);
}
