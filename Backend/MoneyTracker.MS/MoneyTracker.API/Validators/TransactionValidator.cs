namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Validator for TransactionDTO
/// </summary>
public class TransactionValidator : AbstractValidator<TransactionDTO>
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TransactionValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.TransactionCategoryId).NotNull().GreaterThan(-1).WithMessage($"TransactionCategoryId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionTypeId).NotNull().GreaterThan(-1).WithMessage($"TransactionTypeId can not be less than 0 nor empty");
            RuleFor(model => model.BankId).NotNull().GreaterThan(-1).WithMessage($"BankId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionAmount).NotNull().GreaterThan(-1).WithMessage($"TransactionAmount can not be less than 0 nor empty");
            RuleFor(model => model.TransactionDate).NotNull().NotEmpty().InclusiveBetween(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1)).WithMessage($"TransactionDate can not be empty nor exceed more or less than 1 year");
            RuleFor(model => model.TransactionDescription).NotNull().NotEmpty().MaximumLength(150).WithMessage($"TransactionDescription can not be empty nor exceed 150 characters");
        });

        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.TransactionId).NotNull().GreaterThan(-1).WithMessage($"TransactionId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionCategoryId).NotNull().GreaterThan(-1).WithMessage($"TransactionCategoryId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionTypeId).NotNull().GreaterThan(-1).WithMessage($"TransactionTypeId can not be less than 0 nor empty");
            RuleFor(model => model.BankId).NotNull().GreaterThan(-1).WithMessage($"BankId can not be less than 0 nor empty");
            RuleFor(model => model.TransactionAmount).NotNull().GreaterThan(-1).WithMessage($"TransactionAmount can not be less than 0 nor empty");
            RuleFor(model => model.TransactionDate).NotNull().NotEmpty().InclusiveBetween(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1)).WithMessage($"TransactionDate can not be empty nor exceed more or less than 1 year");
            RuleFor(model => model.TransactionDescription).NotNull().NotEmpty().MaximumLength(150).WithMessage($"TransactionDescription can not be empty nor exceed 150 characters");
        });
    }
}