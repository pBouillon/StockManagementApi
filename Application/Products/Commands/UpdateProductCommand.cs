using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    /// <summary>
    /// CQRS command to update a <see cref="Product"/>
    /// </summary>
    public class UpdateProductCommand : IRequest<Product>
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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
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
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="logger">Logger</param>
        public UpdateProductCommandHandler(IApplicationDbContext context, ILogger<UpdateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        /// <summary>
        /// Update a <see cref="Product"/> in the database from the incoming <see cref="UpdateProductCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> updated</returns>
        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

            entity.Name = request.Name;

            _ = _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"{entity} renamed to {request.Name}");

            return entity;
        }
    }
}
