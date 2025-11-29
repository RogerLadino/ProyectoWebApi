using System;
using System.Collections.Concurrent;
using Core.Services.Abstractions;

namespace Services;

public class TokenRevocationService : ITokenRevocationService
{
    private readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

    public void RevokeToken(string jti, DateTime expiresUtc)
    {
        if (string.IsNullOrWhiteSpace(jti))
            throw new ArgumentException("El JTI no puede ser nulo o vacío.", nameof(jti));

        _revokedTokens[jti] = expiresUtc;
        CleanupExpiredTokens();
    }

    public bool IsTokenRevoked(string jti)
    {
        if (string.IsNullOrWhiteSpace(jti))
            return false;

        if (_revokedTokens.TryGetValue(jti, out var expiresUtc))
        {
            if (expiresUtc > DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                _revokedTokens.TryRemove(jti, out _);
            }
        }

        return false;
    }

    private void CleanupExpiredTokens()
    {
        foreach (var kvp in _revokedTokens)
        {
            if (kvp.Value <= DateTime.UtcNow)
            {
                _revokedTokens.TryRemove(kvp.Key, out _);
            }
        }
    }
}
