
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

public class ToDoFilterValidator: AbstractValidator<ToDoListFilter>
{
    public ToDoFilterValidator()
    {
        RuleFor(filter=>filter.pageSize).GreaterThanOrEqualTo(1);
        RuleFor(filter=>filter.page).GreaterThanOrEqualTo(0);
    }
    protected override void RaiseValidationException(ValidationContext<ToDoListFilter> context, ValidationResult result)
    {
        // Extract the first error message or join them all
        var message = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
        
        // Throw your specific exception
        throw new ArgumentException(message);
    }
}