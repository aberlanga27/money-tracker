namespace MoneyTracker.Domain.Common;

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;
using Newtonsoft.Json;

[ExcludeFromCodeCoverage]
public class SecretManager(string api, string clientSecret, string clientId, string workspaceId, string environment)
{
    private readonly string clientSecret = Guard.Against.NullOrEmpty(clientSecret);
    private readonly string clientId = Guard.Against.NullOrEmpty(clientId);
    private readonly string workspaceId = Guard.Against.NullOrEmpty(workspaceId);
    private readonly string environment = Guard.Against.NullOrEmpty(environment);

    private HttpClient HttpClient { get; } = new()
    {
        BaseAddress = new Uri(Guard.Against.NullOrEmpty(api))
    };

    private async Task<string> GetAccessToken()
    {
        var response = await HttpClient.PostAsync(
            "api/v1/auth/universal-auth/login",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["clientSecret"] = clientSecret,
                ["clientId"] = clientId
            })
        );

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<SecretManagerAuth>(content);

        if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
            throw new InvalidOperationException("Failed to retrieve access token from Secret Manager.");

        return authResponse.AccessToken;
    }

    public async Task<string> GetSecret(string secretName)
    {
        var accessToken = await GetAccessToken();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await HttpClient.GetAsync(
            $"api/v3/secrets/raw/{secretName}?workspaceId={workspaceId}&environment={environment}"
        );

        if (!response.IsSuccessStatusCode)
            return string.Empty;

        var content = await response.Content.ReadAsStringAsync();
        var secretResponse = JsonConvert.DeserializeObject<SecretManagerSecretValue>(content);

        if (secretResponse == null || string.IsNullOrEmpty(secretResponse.Secret.SecretValue))
            return string.Empty;

        return secretResponse.Secret.SecretValue;
    }
}