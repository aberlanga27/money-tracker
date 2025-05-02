namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for BudgetTypeDTO
/// </summary>
public class BudgetTypeValidator : AbstractValidator<BudgetTypeDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public BudgetTypeValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.BudgetTypeName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"BudgetTypeName can not be empty nor exceed 100 characters");
            RuleFor(model => model.BudgetTypeDays).NotNull().GreaterThan(-1).WithMessage($"BudgetTypeDays can not be less than 0 nor empty");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.BudgetTypeId).NotNull().GreaterThan(-1).WithMessage($"BudgetTypeId can not be less than 0 nor empty");
            RuleFor(model => model.BudgetTypeName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"BudgetTypeName can not be empty nor exceed 100 characters");
            RuleFor(model => model.BudgetTypeDays).NotNull().GreaterThan(-1).WithMessage($"BudgetTypeDays can not be less than 0 nor empty");
        });
    }
}