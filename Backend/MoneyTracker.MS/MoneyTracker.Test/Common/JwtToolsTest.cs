namespace MoneyTracker.Test.Common;

using System.IdentityModel.Tokens.Jwt;
using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities.Config;
using Xunit;

public class JwtToolsTest
{
    private readonly MoneyTrackerSettings mockSettings;
    private readonly JwtTools jwtTools;

    public JwtToolsTest()
    {
        mockSettings = new MoneyTrackerSettings
        {
            Jwt = new MoneyTrackerSettings.JwtModel
            {
                Key = "testKeytestKeytestKeytestKeytestKey",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpirationInMinutes = 60
            }
        };

        jwtTools = new JwtTools(mockSettings);
    }

    [Fact]
    public void GenerateToken_NoClaims_ReturnsToken()
    {
        var token = jwtTools.GenerateToken();

        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Equal("testIssuer", jwtToken.Issuer);
        Assert.Equal("testAudience", jwtToken.Audiences.First());
    }

    [Fact]
    public void GenerateToken_WithClaims_ReturnsTokenWithClaims()
    {
        var claims = new Dictionary<string, string>
        {
            { "claim1", "value1" },
            { "claim2", "value2" }
        };

        var token = jwtTools.GenerateToken(claims);

        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Equal("testIssuer", jwtToken.Issuer);
        Assert.Equal("testAudience", jwtToken.Audiences.First());
        Assert.Contains(jwtToken.Claims, c => c.Type == "claim1" && c.Value == "value1");
        Assert.Contains(jwtToken.Claims, c => c.Type == "claim2" && c.Value == "value2");
    }
}