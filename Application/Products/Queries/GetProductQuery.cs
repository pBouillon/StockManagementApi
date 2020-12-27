using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetProductQuery : IRequest<Product?>
    {
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<GetProductQueryHandler> _logger;

        public GetProductQueryHandler(IApplicationDbContext context, ILogger<GetProductQueryHandler> logger)
            => (_context, _logger) = (context, logger);

        public Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Products
                .FirstOrDefault(product => product.Id == request.Id);

            _logger.LogDebug($"Product of id { request.Id } retrieved { entity }");

            return Task.FromResult(entity);
        }
    }
}
