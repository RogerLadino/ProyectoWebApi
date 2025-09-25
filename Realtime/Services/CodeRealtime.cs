using Domain.Realtime;
using Microsoft.AspNetCore.SignalR;
using Realtime.Hubs;
using Shared.DTOs.Code;

namespace Realtime.Services;

public class CodeRealtime: ICodeRealtime
{
    private readonly IHubContext<CodeHub> _hub;

    public CodeRealtime(IHubContext<CodeHub> hub)
    {
        _hub = hub;
    }

    public Task NotifyCodeChangedAsync(CodeUpdateDto code)
    {
        return _hub.Clients.Group($"exercise-{code.ExerciseId}-{code.AppUserId}")
            .SendAsync("CodeUpdated", code);
    }
}
