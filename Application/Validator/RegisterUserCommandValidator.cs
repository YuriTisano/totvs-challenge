using Application.Command;
using FluentValidation;

namespace Application.Validator
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("User email is required.")
                 .MaximumLength(100).WithMessage("user email must not exceed 100 characters.")
                 .EmailAddress().WithMessage("User email must be a valid email address.");

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("User password is required.")
                 .MaximumLength(50).WithMessage("user password must not exceed 50 characters.")
                 .MinimumLength(8).WithMessage("User password must have at least 8 characters.")
                 .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""':{}|<>]).{8,}$").WithMessage("Password must have at least 8 characters, including uppercase, lowercase, number, and special character.");
        }
    }
}
