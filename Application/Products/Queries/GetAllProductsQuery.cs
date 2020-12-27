using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IApplicationDbContext _context;
        
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IApplicationDbContext context, ILogger<GetAllProductsQueryHandler> logger)
            => (_context, _logger) = (context, logger);

        public Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.Products.AsEnumerable();

            _logger.LogDebug("All product retrieved");

            return Task.FromResult(entities);
        }
    }
}
