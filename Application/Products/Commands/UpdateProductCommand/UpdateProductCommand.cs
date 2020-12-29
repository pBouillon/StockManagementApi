using Application.Commons.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Exceptions;
using Application.Products.Dtos;

namespace Application.Products.Commands.UpdateProductCommand
{
    /// <summary>
    /// CQRS command to update a <see cref="Product"/>
    /// </summary>
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        /// <summary>
        /// Id of the <see cref="Product"/> to update
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="UpdateProductCommand"/>
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        /// <summary>
        /// AutoMapper interface to map domain objects to DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="mapper">AutoMapper interface to map domain objects to DTO</param>
        /// <param name="logger">Logger</param>
        public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<UpdateProductCommandHandler> logger)
            => (_context, _mapper, _logger) = (context, mapper, logger);

        /// <summary>
        /// Update a <see cref="Product"/> in the database from the incoming <see cref="UpdateProductCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> updated mapped to a <see cref="ProductDto"/></returns>
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.Id, cancellationToken);
            
            if (entity == null)
            {
                var unknownProductException = new NotFoundException(nameof(Product), new { request.Id });

                _logger.LogError(unknownProductException, $"No product found for the provided id {request.Id}");

                throw unknownProductException;
            }

            entity.Name = request.Name;

            _ = _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"{entity} renamed to {request.Name}");

            return _mapper.Map<ProductDto>(entity);
        }
    }
}
