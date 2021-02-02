using FluentValidation;
using static Application.Commons.Models.Identity.User;

namespace Application.User.Commands.UpdateUserCommand
{
    /// <summary>
    /// Validator used to validate the update of a user
    /// </summary>
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        /// <summary>
        /// Create the validator
        /// </summary>
        public UpdateUserCommandValidator()
        {
            RuleFor(command => command.Username)
                .MinimumLength(UsernameMinimumLength)
                .WithMessage($"The username must contains at least {UsernameMinimumLength} characters")
                .MaximumLength(UsernameMaximumLength)
                .WithMessage($"The username can contains at most {UsernameMaximumLength} characters");
        }
    }
}
