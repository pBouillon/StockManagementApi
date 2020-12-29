using AutoMapper;
using Domain.Entities;

namespace Application.Products.Dtos
{
    /// <summary>
    /// <see cref="Product"/> mappings to DTOs
    /// </summary>
    public class ProductMapping : Profile
    {
        /// <summary>
        /// Create the <see cref="Product"/> mappings
        /// </summary>
        public ProductMapping()
        {
            // Map a single Product to its DTO
            CreateMap<Product, ProductDto>();
        }
    }
}
