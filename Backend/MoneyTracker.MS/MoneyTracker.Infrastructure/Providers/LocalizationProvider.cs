namespace MoneyTracker.Infrastructure.Providers;

using System.Diagnostics.CodeAnalysis;
using System.IO;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;

[ExcludeFromCodeCoverage]
public class LocalizationProvider(
    MoneyTrackerSettings appSettings,
    ApiConfiguration apiConfiguration
) : ILocalizationProvider
{
    private readonly MoneyTrackerSettings appSettings = Guard.Against.Null(appSettings);
    private readonly ApiConfiguration apiConfiguration = Guard.Against.Null(apiConfiguration);
    private readonly string defaultLanguage = Guard.Against.NullOrEmpty(appSettings.Localization.Default, "Localization:Default", "Default localization is required");
    private readonly Dictionary<string, IDictionary<string, string>?> localizations = LoadLocalizationsFromJson(appSettings.Localization.Supported);

    private static Dictionary<string, IDictionary<string, string>?> LoadLocalizationsFromJson(string[] supportedLanguages)
    {
        var localizations = new Dictionary<string, IDictionary<string, string>?>();

        foreach (var langName in supportedLanguages)
        {
            using var stream = File.OpenRead($"./Languages/{langName}.json");
            using var reader = new StreamReader(stream);
            var fileContent = reader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(fileContent))
                continue;

            localizations[langName] = JsonConvert.DeserializeObject<IDictionary<string, string>?>(fileContent);
        }

        return localizations;
    }

    private string TryGetT(IDictionary<string, string>? localization, string key)
    {
        return localization?.TryGetValue(key, out var localizedValue) ?? false
            ? localizedValue
            : key;
    }

    public string T(string language, string key)
    {
        return localizations.TryGetValue(language, out var localization)
            ? TryGetT(localization, key)
            : key;
    }

    public string T(string language, string key, List<string> values)
    {
        var localizedValue = T(language, key);
        values = values.Select(value => T(language, value)).ToList();

        for (var i = 0; i < values.Count; i++)
            localizedValue = localizedValue.Replace($"{{{i}}}", values[i]);

        return localizedValue;
    }

    public string T(string key)
    {
        var language = apiConfiguration.Language ?? defaultLanguage;
        return T(language, key);
    }

    public string T(string key, List<string> values)
    {
        var language = apiConfiguration.Language ?? defaultLanguage;
        return T(language, key, values);
    }
}