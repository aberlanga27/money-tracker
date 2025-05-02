namespace MoneyTracker.Domain.Interfaces;

public interface ILocalizationProvider
{
    /// <summary>
    /// Translate a key to a specific language
    /// </summary>
    /// <param name="language"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    string T(string language, string key);

    /// <summary>
    /// Translate a key to a specific language with values
    /// </summary>
    /// <param name="language"></param>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    string T(string language, string key, List<string> values);

    /// <summary>
    /// Translate a key to the default language
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string T(string key);

    /// <summary>
    /// Translate a key to the default language with values
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    string T(string key, List<string> values);
}