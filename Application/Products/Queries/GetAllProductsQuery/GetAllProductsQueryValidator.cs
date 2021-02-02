using Domain.Entities;
using FluentValidation;

namespace Application.Products.Queries.GetAllProductsQuery
{
    /// <summary>
    /// Validator used to validate the query of all <see cref="Product"/>
    /// </summary>
    public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        /// <summary>
        /// Maximum number of items per pages that will be tolerated
        /// </summary>
        /// <remarks>
        /// The bound is inclusive
        /// </remarks>
        private const int MaximumNumberOfItemsPerPages = 50;

        /// <summary>
        /// Create the validator
        /// </summary>
        public GetAllProductsQueryValidator()
        {
            RuleFor(query => query.PageSize)
                .GreaterThan(0)
                .WithMessage("A page must contains at least 1 item")
                .LessThanOrEqualTo(MaximumNumberOfItemsPerPages)
                .WithMessage($"A maximum of {MaximumNumberOfItemsPerPages} items per pages can be fetched");

            RuleFor(query => query.PageNumber)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The page number must be at least 0");
        }
    }
}
