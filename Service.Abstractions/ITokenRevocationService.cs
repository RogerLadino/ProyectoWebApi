using System;

namespace Core.Services.Abstractions;

public interface ITokenRevocationService
{
    void RevokeToken(string jti, DateTime expiresUtc);
    bool IsTokenRevoked(string jti);
}
