using Domain.Entities;

namespace Application.Commons.Dtos
{
    /// <summary>
    /// <see cref="Product"/> DTO
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// If of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
