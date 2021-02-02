using Domain.Entities;
using FluentValidation;

namespace Application.Products.Commands.UpdateProductCommand
{
    /// <summary>
    /// Validator used to validate the update of a <see cref="Product"/>
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        /// <summary>
        /// Create the validator
        /// </summary>
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("A product should have a name")
                .MaximumLength(Product.NameMaxLength)
                .WithMessage($"The product's name cannot exceed {Product.NameMaxLength} characters");
        }
    }
}
