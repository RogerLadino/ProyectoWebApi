using Shared.DTOs.Code;

namespace Domain.Realtime;

public interface ICodeRealtime
{
    Task NotifyCodeChangedAsync(CodeUpdateDto code);
}
