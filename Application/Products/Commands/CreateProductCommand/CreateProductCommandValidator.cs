using Domain.Entities;
using FluentValidation;

namespace Application.Products.Commands.CreateProductCommand
{
    /// <summary>
    /// TODO
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        /// <summary>
        /// TODO
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
