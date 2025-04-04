using FluentValidation;
using Opulenza.Application.Models;

namespace Opulenza.Application.Validators;

public class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(email => email.To)
            .NotEmpty().WithMessage("Recipient email address is required.")
            .EmailAddress().WithMessage("Invalid email address format.");

        RuleFor(email => email.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(100).WithMessage("Subject cannot exceed 100 characters.");

        RuleFor(email => email.Body)
            .NotEmpty().WithMessage("Email body cannot be empty.");

        RuleFor(email => email.IsBodyHtml)
            .NotNull().WithMessage("IsBodyHtml must be specified.");
    }
}