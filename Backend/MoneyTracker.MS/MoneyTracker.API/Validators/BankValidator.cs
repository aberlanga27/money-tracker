namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for BankDTO
/// </summary>
public class BankValidator : AbstractValidator<BankDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public BankValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.BankName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"BankName can not be empty nor exceed 100 characters");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.BankId).NotNull().GreaterThan(-1).WithMessage($"BankId can not be less than 0 nor empty");
            RuleFor(model => model.BankName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"BankName can not be empty nor exceed 100 characters");
        });
    }
}