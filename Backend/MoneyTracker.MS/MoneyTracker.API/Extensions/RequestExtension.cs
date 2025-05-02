namespace MoneyTracker.API.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Request extension
/// </summary>
[ExcludeFromCodeCoverage]
public static class RequestExtension
{
    /// <summary>
    /// Get header value
    /// </summary>
    /// <param name="request"></param>
    /// <param name="headerName"></param>
    /// <param name="headerValue"></param>
    /// <returns></returns>
    public static bool GetHeaderValue(this HttpRequest request, string headerName, out string headerValue)
    {
        headerValue = request?.Headers[headerName].FirstOrDefault() ?? string.Empty;
        return !string.IsNullOrEmpty(headerValue);
    }

    /// <summary>
    /// Get JWT token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool GetJwtToken(this HttpRequest request, out string token)
    {
        token = string.Empty;
        if (!request.GetHeaderValue("Authorization", out var authorization))
            return false;

        token = authorization.Replace("Bearer ", string.Empty);
        return true;
    }

    /// <summary>
    /// Get JWT claim by name
    /// </summary>
    /// <param name="request"></param>
    /// <param name="claimName"></param>
    /// <param name="claimValue"></param>
    /// <returns></returns>
    public static bool GetJwtClaimByName(this HttpRequest request, string claimName, out string claimValue)
    {
        claimValue = string.Empty;
        if (!request.GetJwtToken(out var token))
            return false;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        claimValue = jwtToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value ?? string.Empty;
        return !string.IsNullOrEmpty(claimValue);
    }
}