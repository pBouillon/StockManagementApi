using Application.Commons.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.Mappings
{
    /// <summary>
    /// <see cref="Product"/> mappings
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
