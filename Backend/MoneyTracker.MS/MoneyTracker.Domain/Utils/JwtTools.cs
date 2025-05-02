namespace MoneyTracker.Domain.Utils;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[ExcludeFromCodeCoverage]
public static class JwtTools
{
    public static string? GenerateToken(string secretKey, string issuer, string audience,
        int expirationInMinutes, Dictionary<string, string>? claims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var defaultClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Iss, issuer),
            new(JwtRegisteredClaimNames.Aud, audience),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(
                JwtRegisteredClaimNames.Exp,
                DateTime.Now.AddMinutes(expirationInMinutes).ToString(CultureInfo.InvariantCulture)
            )
        };

        if (claims != null)
            foreach (var claim in claims)
                defaultClaims = [.. defaultClaims, new Claim(claim.Key, claim.Value)];

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: defaultClaims,
            expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}