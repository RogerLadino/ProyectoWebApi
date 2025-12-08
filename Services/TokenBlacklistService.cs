using System.Collections.Concurrent;
using Service.Abstractions;
using Core.Services;
using Core.Services.Abstractions;
using Domain.Realtime;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Services;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

    public void RevokeToken(string token)
    {
        _revokedTokens.TryAdd(token, DateTime.UtcNow);
    }

    public bool IsTokenRevoked(string token)
    {
        return _revokedTokens.ContainsKey(token);
    }
}
