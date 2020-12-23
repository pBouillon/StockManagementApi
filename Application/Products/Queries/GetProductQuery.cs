using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries
{
    public class GetProductQuery : IRequest<Product?>
    {
        public int ProductId { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<GetProductQueryHandler> _logger;

        public GetProductQueryHandler(IApplicationDbContext context, ILogger<GetProductQueryHandler> logger)
            => (_context, _logger) = (context, logger);

        public Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_context.Products.FirstOrDefault(product => product.Id == request.ProductId));
    }
}
