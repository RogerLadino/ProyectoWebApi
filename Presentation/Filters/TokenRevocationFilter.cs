using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Abstractions;

public class TokenRevocationFilter : IAuthorizationFilter
{
    private readonly ITokenBlacklistService _tokenBlacklistService;

    public TokenRevocationFilter(ITokenBlacklistService tokenBlacklistService)
    {
        _tokenBlacklistService = tokenBlacklistService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token) && _tokenBlacklistService.IsTokenRevoked(token))
        {
            context.Result = new UnauthorizedObjectResult("Token revocado.");
        }
    }
}
