using Domain.Entities;
using FluentValidation;

namespace Application.Products.Commands.UpdateProductCommand
{
    /// <summary>
    /// TODO
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        /// <summary>
        /// TODO
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
