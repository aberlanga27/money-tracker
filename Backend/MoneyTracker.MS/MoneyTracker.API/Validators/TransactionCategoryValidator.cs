namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for TransactionCategoryDTO
/// </summary>
public class TransactionCategoryValidator : AbstractValidator<TransactionCategoryDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TransactionCategoryValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.TransactionCategoryName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryName can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryDescription).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryDescription can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryIcon).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryIcon can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryColor).NotNull().NotEmpty().MaximumLength(6).WithMessage($"TransactionCategoryColor can not be empty nor exceed 6 characters");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.TransactionCategoryId).NotNull().GreaterThan(-1).WithMessage($"TransactionCategoryId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionCategoryName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryName can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryDescription).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryDescription can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryIcon).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionCategoryIcon can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionCategoryColor).NotNull().NotEmpty().MaximumLength(6).WithMessage($"TransactionCategoryColor can not be empty nor exceed 6 characters");
        });
    }
}