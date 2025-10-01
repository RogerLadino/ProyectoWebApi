using Microsoft.AspNetCore.SignalR;
using Service.Abstractions;
using Shared.DTOs.Code;

namespace Realtime.Hubs;

public class CodeHub : Hub
{
    private readonly IServiceManager _serviceManager;

    public CodeHub(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task UpdateCode(CodeUpdateDto code)
    {
        await _serviceManager.CodeService.UpdateCodeAsync(code);
    }

    public async Task JoinExerciseGroup(int exerciseId, int userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"exercise-{exerciseId}-{userId}");

        var code = await _serviceManager.CodeService.GetByIdAsync(exerciseId, userId);

        await Clients.Caller.SendAsync("CodeInitialized", code);
    }
}
