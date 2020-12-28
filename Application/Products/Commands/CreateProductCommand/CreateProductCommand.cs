using Application.Commons.Dtos;
using Application.Commons.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands.CreateProductCommand
{
    /// <summary>
    /// CQRS command to create a <see cref="Product"/>
    /// </summary>
    public class CreateProductCommand : IRequest<ProductDto>
    {
        /// <summary>
        /// Name of the <see cref="Product"/> to be created
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="CreateProductCommand"/>
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<CreateProductCommandHandler> _logger;

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
        public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
            => (_context, _mapper, _logger) = (context, mapper, logger);

        /// <summary>
        /// Add a <see cref="Product"/> to the database from the incoming <see cref="CreateProductCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> created mapped to a <see cref="ProductDto"/></returns>
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Name = request.Name,
            };

            await _context.Products.AddAsync(entity, cancellationToken);

            _ = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"{entity} successfully created");

            return _mapper.Map<ProductDto>(entity);
        }
    }
}
