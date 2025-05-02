namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for TransactionTypeDTO
/// </summary>
public class TransactionTypeValidator : AbstractValidator<TransactionTypeDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TransactionTypeValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.TransactionTypeName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionTypeName can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionTypeDescription).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionTypeDescription can not be empty nor exceed 100 characters");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.TransactionTypeId).NotNull().GreaterThan(-1).WithMessage($"TransactionTypeId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionTypeName).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionTypeName can not be empty nor exceed 100 characters");
            RuleFor(model => model.TransactionTypeDescription).NotNull().NotEmpty().MaximumLength(100).WithMessage($"TransactionTypeDescription can not be empty nor exceed 100 characters");
        });
    }
}