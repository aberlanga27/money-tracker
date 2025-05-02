namespace MoneyTracker.Domain.Common;

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.IdentityModel.Tokens;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;

public class JwtTools(
    MoneyTrackerSettings appSettings
) : IJwtTools
{
    private readonly string SECRETKEY = Guard.Against.Null(appSettings.Jwt.Key);
    private readonly string ISSUER = Guard.Against.Null(appSettings.Jwt.Issuer);
    private readonly string AUDIENCE = Guard.Against.Null(appSettings.Jwt.Audience);
    private readonly int EXPIRATION = Guard.Against.Null(appSettings.Jwt.ExpirationInMinutes);

    public string? GenerateToken(Dictionary<string, string>? claims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var defaultClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Iss, ISSUER),
            new(JwtRegisteredClaimNames.Aud, AUDIENCE),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(
                JwtRegisteredClaimNames.Exp,
                DateTime.Now.AddMinutes(EXPIRATION).ToString(CultureInfo.InvariantCulture)
            )
        };

        if (claims != null)
            foreach (var claim in claims)
                defaultClaims = [.. defaultClaims, new Claim(claim.Key, claim.Value)];

        var token = new JwtSecurityToken(
            issuer: ISSUER,
            audience: AUDIENCE,
            claims: defaultClaims,
            expires: DateTime.UtcNow.AddMinutes(EXPIRATION),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}