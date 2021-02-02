using FluentValidation;
using static Application.Commons.Models.Identity.User;

namespace Application.User.Commands.CreateUserCommand
{
    /// <summary>
    /// Validator used to validate the creation of a user
    /// </summary>
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        /// <summary>
        /// Create the validator
        /// </summary>
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.Username)
                .MinimumLength(UsernameMinimumLength)
                .WithMessage($"The username must contains at least {UsernameMinimumLength} characters")
                .MaximumLength(UsernameMaximumLength)
                .WithMessage($"The username can contains at most {UsernameMaximumLength} characters");
        }
    }
}
