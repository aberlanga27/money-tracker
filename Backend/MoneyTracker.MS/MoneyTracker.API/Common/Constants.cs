namespace MoneyTracker.API.Common;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Constants used in the API
/// </summary>
[ExcludeFromCodeCoverage]
public static class Constants
{
    /// <summary>
    /// Base path for the API
    /// </summary>
    public const string BasePath = "/api/v1";

    /// <summary>
    /// RuleSet for the insert of the entities using FluentValidation
    /// </summary>
    public const string InsertRuleSet = "Insert";

    /// <summary>
    /// RuleSet for the update of the entities using FluentValidation
    /// </summary>
    public const string UpdateRuleSet = "Update";
}