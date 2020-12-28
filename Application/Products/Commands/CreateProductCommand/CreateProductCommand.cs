using Application.Commons.Interfaces;
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
    public class CreateProductCommand : IRequest<Product>
    {
        /// <summary>
        /// Name of the <see cref="Product"/> to be created
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="CreateProductCommand"/>
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
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
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="logger">Logger</param>
        public CreateProductCommandHandler(IApplicationDbContext context, ILogger<CreateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        /// <summary>
        /// Add a <see cref="Product"/> to the database from the incoming <see cref="CreateProductCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> created</returns>
        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Name = request.Name,
            };

            await _context.Products.AddAsync(entity, cancellationToken);

            _ = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"{entity} successfully created");

            return entity;
        }
    }
}
