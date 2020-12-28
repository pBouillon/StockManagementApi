using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetProductQuery
{
    /// <summary>
    /// CQRS query to retrieve a specific <see cref="Product"/>
    /// </summary>
    public class GetProductQuery : IRequest<Product?>
    {
        /// <summary>
        /// Id of the product to retrieve
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// Handler for the <see cref="GetProductQuery"/>
    /// </summary>
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<GetProductQueryHandler> _logger;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="logger">Logger</param>
        public GetProductQueryHandler(IApplicationDbContext context, ILogger<GetProductQueryHandler> logger)
            => (_context, _logger) = (context, logger);

        /// <summary>
        /// Retrieve a specific <see cref="Product"/> in the database, based on the incoming
        /// <see cref="GetProductQuery"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> if any matches the query; null otherwise</returns>
        public Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Products
                .FirstOrDefault(product => product.Id == request.Id);

            _logger.LogDebug($"Product of id { request.Id } retrieved { entity }");

            return Task.FromResult(entity);
        }
    }
}
