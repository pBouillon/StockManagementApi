using Domain.Entities;
using FluentValidation;

namespace Application.Products.Commands.CreateProductCommand
{
    /// <summary>
    /// Validator used to validate the creation of a <see cref="Product"/>
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        /// <summary>
        /// Create the validator
        /// </summary>
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("A product should have a name")
                .MaximumLength(Product.NameMaxLength)
                .WithMessage($"The product's name cannot exceed {Product.NameMaxLength} characters");
        }
    }
}
