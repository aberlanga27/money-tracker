namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for BudgetDTO
/// </summary>
public class BudgetValidator : AbstractValidator<BudgetDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public BudgetValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.TransactionCategoryId).NotNull().GreaterThan(-1).WithMessage($"TransactionCategoryId can not be less than 0 nor empty");
            RuleFor(model => model.BudgetTypeId).NotNull().GreaterThan(-1).WithMessage($"BudgetTypeId can not be less than 0 nor empty");
            RuleFor(model => model.BudgetAmount).NotNull().GreaterThan(-1).WithMessage($"BudgetAmount can not be less than 0 nor empty");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.BudgetId).NotNull().GreaterThan(-1).WithMessage($"BudgetId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionCategoryId).NotNull().GreaterThan(-1).WithMessage($"TransactionCategoryId can not be less than 0 nor empty");
            RuleFor(model => model.BudgetTypeId).NotNull().GreaterThan(-1).WithMessage($"BudgetTypeId can not be less than 0 nor empty");
            RuleFor(model => model.BudgetAmount).NotNull().GreaterThan(-1).WithMessage($"BudgetAmount can not be less than 0 nor empty");
        });
    }
}