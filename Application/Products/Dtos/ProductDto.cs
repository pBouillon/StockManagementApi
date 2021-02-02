using Domain.Entities;

namespace Application.Products.Dtos
{
    /// <summary>
    /// <see cref="Product"/> DTO
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
