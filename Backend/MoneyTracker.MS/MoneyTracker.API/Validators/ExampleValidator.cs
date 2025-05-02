namespace MoneyTracker.API.Validator;

using FluentValidation;
using MoneyTracker.API.Common;
using MoneyTracker.Domain.DTOs;

/// <summary>
/// Example validator
/// </summary>
public class ExampleValidator : AbstractValidator<ExampleDTO>
{
    /// <summary>
    /// Constructor for the ExampleValidator
    /// </summary>
    public ExampleValidator()
    {
        RuleSet(Constants.InsertRuleSet, () =>
        {
            RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Name Can Not be null Or empty");
            RuleFor(model => model.Description).NotNull().NotEmpty().WithMessage("Description Can Not be null Or empty");
            RuleFor(model => model.Url).NotNull().NotEmpty().WithMessage("Url Can Not be null Or empty");
        });
        RuleSet(Constants.UpdateRuleSet, () =>
        {
            RuleFor(model => model.ExampleId).NotNull().NotEmpty()
                .WithMessage("ExampleId Can Not be null Or empty")
                .GreaterThan(0).WithMessage("ExampleId Can Not be less than 0");
            RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("Name Can Not be null Or empty");
            RuleFor(model => model.Description).NotNull().NotEmpty().WithMessage("Description Can Not be null Or empty");
            RuleFor(model => model.Url).NotNull().NotEmpty().WithMessage("Url Can Not be null Or empty");
        });
    }
}