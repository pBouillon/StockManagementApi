using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Exceptions;

namespace Application.Products.Commands.DeleteProductCommand
{
    /// <summary>
    /// CQRS command to delete a <see cref="Product"/>
    /// </summary>
    public class DeleteProductCommand : IRequest
    {
        /// <summary>
        /// Id of the <see cref="Product"/> to delete
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// Handler for the <see cref="DeleteProductCommand"/>
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="logger">Logger</param>
        public DeleteProductCommandHandler(IApplicationDbContext context, ILogger<DeleteProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        /// <summary>
        /// Delete a <see cref="Product"/> in the database from the incoming <see cref="CreateProductCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CreateProductCommand"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>A no-op result</returns>
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

            if (entity == null)
            {
        
                var unknownProductException = new NotFoundException(nameof(Product), new { request.Id });

                _logger.LogError(unknownProductException, $"No product found for the provided id {request.Id}");

                throw unknownProductException;
            }

            _context.Products.Remove(entity);

            _ = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Product {entity} successfully deleted");

            return Unit.Value;
        }
    }
}
