using Domain.Realtime;
using Microsoft.AspNetCore.SignalR;
using Realtime.Hubs;

namespace Realtime.Services;

public sealed class RealtimeManager : IRealtimeManager
{
    private readonly Lazy<ICodeRealtime> _lazyCodeRealtime;

    public RealtimeManager(IHubContext<CodeHub> hubContext)
    {
        _lazyCodeRealtime = new Lazy<ICodeRealtime>(
            () => new CodeRealtime(hubContext)
        );
    }

    public ICodeRealtime CodeRealtime => _lazyCodeRealtime.Value;
}
